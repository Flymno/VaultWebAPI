using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Security.Cryptography;
using System.Text;
using VaultWebAPI.Data.Queries;

namespace VaultWebAPI.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class SQLtest : ControllerBase
    {
        [HttpPost("addUser")]
        public async Task<IActionResult> AddUser([FromServices] IConfiguration config)
        {
            string ConnectionString = config.GetConnectionString("DefaultConnection");
            using NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);

            string access_token = Guid.NewGuid().ToString();
            byte[] input_bytes = Encoding.UTF8.GetBytes(access_token);
            byte[] hashed_bytes = SHA256.HashData(input_bytes);
            string hashed_token = Convert.ToHexString(hashed_bytes);

            // INSERT INTO users VALUES (DEFAULT, Convert.ToHexString(hashed_bytes))
            await connection.ExecuteAsync(SQLStatements.RegisterUser, new { Token = hashed_token });
            
            return Ok(access_token);
        }
    }
}
