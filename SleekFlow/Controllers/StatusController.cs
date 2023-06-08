using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SleekFlow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public StatusController(IConfiguration configuration)
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
            string query = "SELECT * FROM status;";
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

        [HttpGet("{status_id}")]
        public async Task<IActionResult> Get(int status_id)
        {
            string query = "SELECT * FROM status WHERE status_id=@status_id;";
            DataTable table = new DataTable();
            using (var connection = await GetOpenConnectionAsync())
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@status_id", status_id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    table.Load(reader);
                }
            }

            return new JsonResult(table);
        }
    }
}
