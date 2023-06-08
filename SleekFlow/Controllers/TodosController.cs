using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using SleekFlow.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SleekFlow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public TodosController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SleekFlowConn");
        }

        private async Task<NpgsqlConnection> GetOpenConnectionAsync()
        {
            var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string statusFilter, string dueDateFilter, string sortBy)
        {
            string query = @"
                SELECT tos.*, t.tag_id,t.tag_name FROM todos tos LEFT JOIN todo_tag_xref tt ON tos.todo_id=tt.todo_id LEFT JOIN tag t ON tt.tag_id = t.tag_id
            ";

            // Add filtering conditions
            bool hasWhereClause = false;
            if (!string.IsNullOrEmpty(statusFilter))
            {
                query += $" WHERE todo_status = '{statusFilter}'";
                hasWhereClause = true;
            }
            if (!string.IsNullOrEmpty(dueDateFilter))
            {
                query += hasWhereClause ? " AND" : " WHERE";
                query += $" todo_due_date = '{dueDateFilter}'";
                hasWhereClause = true;
            }

            // Add sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                query += $" ORDER BY {sortBy}";
            }

            List<Todo> todos = new List<Todo>();

            using (var connection = await GetOpenConnectionAsync())
            using (var command = new NpgsqlCommand(query, connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(reader.GetOrdinal("todo_id"));
                    string name = reader.GetString(reader.GetOrdinal("todo_name"));
                    string description = reader.GetString(reader.GetOrdinal("todo_description"));
                    string status = reader.GetString(reader.GetOrdinal("todo_status"));
                    string dueDate = reader.GetDateTime(reader.GetOrdinal("todo_due_date")).ToString().Split(' ')[0];
                    int tagId = reader.IsDBNull(reader.GetOrdinal("tag_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("tag_id"));
                    string tagName = reader.IsDBNull(reader.GetOrdinal("tag_name")) ? null : reader.GetString(reader.GetOrdinal("tag_name"));

                    Todo todo = todos.FirstOrDefault(t => t.todo_id == id);

                    if (todo == null)
                    {
                        todo = new Todo
                        {
                            todo_id = id,
                            todo_name = name,
                            todo_description = description,
                            todo_status = status,
                            todo_due_date = dueDate,
                            todo_tags = new List<Tag>()
                        };

                        todos.Add(todo);
                    }

                    if (!string.IsNullOrEmpty(tagName) && tagId>0)
                    {

                        todo.todo_tags.Add(new Tag { tag_id=tagId, tag_name=tagName});
                    }
                }
            }

            return new JsonResult(todos);
        }

        [HttpGet("{todo_id}")]
        public async Task<IActionResult> Get(int todo_id)
        {
            string query = "SELECT tos.*, t.tag_id,t.tag_name FROM todos tos LEFT JOIN todo_tag_xref tt ON tos.todo_id=tt.todo_id LEFT JOIN tag t ON tt.tag_id = t.tag_id WHERE tos.todo_id=@todo_id;";

            List<Todo> todos = new List<Todo>();

            using (var connection = await GetOpenConnectionAsync())
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@todo_id", todo_id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("todo_id"));
                        string name = reader.GetString(reader.GetOrdinal("todo_name"));
                        string description = reader.GetString(reader.GetOrdinal("todo_description"));
                        string status = reader.GetString(reader.GetOrdinal("todo_status"));
                        string dueDate = reader.GetDateTime(reader.GetOrdinal("todo_due_date")).ToString().Split(' ')[0];
                        int tagId = reader.IsDBNull(reader.GetOrdinal("tag_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("tag_id"));
                        string tagName = reader.IsDBNull(reader.GetOrdinal("tag_name")) ? null : reader.GetString(reader.GetOrdinal("tag_name"));

                        Todo todo = todos.FirstOrDefault(t => t.todo_id == id);

                        if (todo == null)
                        {
                            todo = new Todo
                            {
                                todo_id = id,
                                todo_name = name,
                                todo_description = description,
                                todo_status = status,
                                todo_due_date = dueDate,
                                todo_tags = new List<Tag>()
                            };

                            todos.Add(todo);
                        }

                        if (!string.IsNullOrEmpty(tagName) && tagId > 0)
                        {

                            todo.todo_tags.Add(new Tag { tag_id = tagId, tag_name = tagName });
                        }
                    }
                }
            }
            

            return new JsonResult(todos);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Todo todo)
        {
            string query = @"
                INSERT INTO todos(todo_name, todo_description, todo_due_date, todo_status) 
                VALUES (@todo_name, @todo_description, DATE(@todo_due_date), @todo_status);
            ";

            using (var connection = await GetOpenConnectionAsync())
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@todo_name", todo.todo_name);
                command.Parameters.AddWithValue("@todo_description", todo.todo_description);
                command.Parameters.AddWithValue("@todo_due_date", todo.todo_due_date);
                command.Parameters.AddWithValue("@todo_status", todo.todo_status);

                var todoId = await command.ExecuteScalarAsync();

                // Insert the associated todo_tags
                if (todo.todo_tags != null && todo.todo_tags.Count > 0)
                {
                    string tagsQuery = @"
                        INSERT INTO todo_tag_xref(todo_id, tag_id)
                        VALUES (@todo_id, @tag_id);
                    ";

                    using (var tagsCommand = new NpgsqlCommand(tagsQuery, connection))
                    {
                        tagsCommand.Parameters.AddWithValue("@todo_id", todoId);

                        foreach (Tag tag in todo.todo_tags)
                        {
                            tagsCommand.Parameters.Clear();
                            tagsCommand.Parameters.AddWithValue("@todo_id", todoId);
                            tagsCommand.Parameters.AddWithValue("@tag_id", tag.tag_id);
                            await tagsCommand.ExecuteNonQueryAsync();
                        }
                    }
                }
            }

            return new JsonResult("Added Successfully");
        }

        [HttpPut]
        public async Task<IActionResult> Put(Todo todo)
        {
            using (var connection = await GetOpenConnectionAsync())
            {
                // Delete existing todo_tags for the given todo_id
                string deleteTagsQuery = @"
                    DELETE FROM todo_tag_xref
                    WHERE todo_id = @todo_id;
                ";

                using (var deleteTagsCommand = new NpgsqlCommand(deleteTagsQuery, connection))
                {
                    deleteTagsCommand.Parameters.AddWithValue("@todo_id", todo.todo_id);
                    await deleteTagsCommand.ExecuteNonQueryAsync();
                }

                // Update the todo record
                string updateTodoQuery = @"
                    UPDATE todos 
                    SET todo_name = @todo_name, todo_description = @todo_description, 
                        todo_due_date = DATE(@todo_due_date), todo_status = @todo_status 
                    WHERE todo_id = @todo_id;
                ";

                using (var updateTodoCommand = new NpgsqlCommand(updateTodoQuery, connection))
                {
                    updateTodoCommand.Parameters.AddWithValue("@todo_id", todo.todo_id);
                    updateTodoCommand.Parameters.AddWithValue("@todo_name", todo.todo_name);
                    updateTodoCommand.Parameters.AddWithValue("@todo_description", todo.todo_description);
                    updateTodoCommand.Parameters.AddWithValue("@todo_due_date", todo.todo_due_date);
                    updateTodoCommand.Parameters.AddWithValue("@todo_status", todo.todo_status);

                    await updateTodoCommand.ExecuteNonQueryAsync();
                }

                // Insert the new todo_tags
                if (todo.todo_tags != null && todo.todo_tags.Count > 0)
                {
                    string insertTagsQuery = @"
                        INSERT INTO todo_tag_xref(todo_id, tag_id)
                        VALUES (@todo_id, @tag_id);
                    ";

                    foreach (Tag tag in todo.todo_tags)
                    {
                        using (var insertTagsCommand = new NpgsqlCommand(insertTagsQuery, connection))
                        {
                            insertTagsCommand.Parameters.AddWithValue("@todo_id", todo.todo_id);
                            insertTagsCommand.Parameters.AddWithValue("@tag_id", tag.tag_id);
                            await insertTagsCommand.ExecuteNonQueryAsync();
                        }
                    }
                }
            }

            return new JsonResult("Updated Successfully");
        }

        [HttpDelete("{todo_id}")]
        public async Task<IActionResult> Delete(int todo_id)
        {
            string query = "DELETE FROM todos WHERE todo_id=@todo_id;";

            using (var connection = await GetOpenConnectionAsync())
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@todo_id", todo_id);

                await command.ExecuteNonQueryAsync();
            }

            return new JsonResult("Deleted Successfully");
        }
    }
}