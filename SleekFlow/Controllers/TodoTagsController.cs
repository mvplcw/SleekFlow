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
    public class TodoTagsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public TodoTagsController(IConfiguration configuration)
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

        [HttpGet("{todo_id}")]
        public async Task<IActionResult> Get(int todo_id)
        {
            string query = "SELECT t.tag_id, t.tag_name FROM todo_tag_xref tt LEFT JOIN tag t ON tt.tag_id = t.tag_id WHERE tt.todo_id = @todo_id;";
            DataTable table = new DataTable();
            using (var connection = await GetOpenConnectionAsync())
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@todo_id", todo_id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    table.Load(reader);
                }
            }

            string sqlDataSource = _configuration.GetConnectionString("SleekFlowConn");

            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
            {
                myCommand.Parameters.AddWithValue("@todo_id", todo_id);

                await myCon.OpenAsync();
                using (NpgsqlDataReader myReader = await myCommand.ExecuteReaderAsync())
                {
                    table.Load(myReader);
                }
            }

            return new JsonResult(table);
        }

        [HttpPost]
        public async Task<IActionResult> Post(TodoTag todoTag)
        {
            string query = "INSERT INTO todo_tag_xref(todo_id, tag_id) VALUES (@todo_id, @tag_id);";
            DataTable table = new DataTable();
            using (var connection = await GetOpenConnectionAsync())
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@todo_id", todoTag.todo_id);
                command.Parameters.AddWithValue("@tag_id", todoTag.tag_id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    table.Load(reader);
                }
            }

            return new JsonResult("Added Successfully");
        }

        [HttpDelete("{todo_id}/{tag_id}")]
        public async Task<IActionResult> Delete(int todo_id, int tag_id)
        {
            string query = "DELETE FROM todo_tag_xref WHERE todo_id = @todo_id AND tag_id = @tag_id;";
            DataTable table = new DataTable();
            using (var connection = await GetOpenConnectionAsync())
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@todo_id", todo_id);
                command.Parameters.AddWithValue("@tag_id", tag_id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    table.Load(reader);
                }
            }

            return new JsonResult("Deleted Successfully");
        }
    }
}
