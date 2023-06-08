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
    public class TagsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public TagsController(IConfiguration configuration)
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
        public async Task<IActionResult> Get()
        {
            string query = "SELECT * FROM tag;";
            DataTable table = new DataTable();
            using (var connection = await GetOpenConnectionAsync())
            using (var command = new NpgsqlCommand(query, connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    table.Load(reader);
                }
            }

            return new JsonResult(table);
        }

        [HttpGet("{tag_id}")]
        public async Task<IActionResult> Get(int tag_id)
        {
            string query = "SELECT * FROM tag WHERE tag_id=@tag_id;";
            DataTable table = new DataTable();
            using (var connection = await GetOpenConnectionAsync())
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@tag_id", tag_id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    table.Load(reader);
                }
            }

            return new JsonResult(table);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Tag tag)
        {
            string query = "INSERT INTO tag(tag_name) VALUES (@tag_name);";
            DataTable table = new DataTable();
            using (var connection = await GetOpenConnectionAsync())
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@tag_name", tag.tag_name);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    table.Load(reader);
                }
            }

            return new JsonResult("Added Successfully");
        }

        [HttpDelete("{tag_id}")]
        public async Task<IActionResult> Delete(int tag_id)
        {
            string query = "DELETE FROM tag WHERE tag_id=@tag_id;";
            DataTable table = new DataTable();
            using (var connection = await GetOpenConnectionAsync())
            using (var command = new NpgsqlCommand(query, connection))
            {
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
