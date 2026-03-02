using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Security.Cryptography;
using System.Text;
using VaultWebAPI.Data.Queries;
using VaultWebAPI.Utility;

namespace VaultWebAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserManagement : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromServices] IConfiguration Config)
        { 
            string ConnectionString = Config.GetConnectionString("DefaultConnection");
            using NpgsqlConnection Connection = new NpgsqlConnection(ConnectionString);

            string Token = Guid.NewGuid().ToString();
            string HashToken = Hashing.Hash(Token);

            // INSERT INTO users VALUES (DEFAULT, HashToken)
            await Connection.ExecuteAsync(SQLStatements.RegisterUser, new { Token = HashToken });

            return Ok(Token);
        }

        public class UserRequest { public string Token { get; set; } }
        [HttpPost("remove")]
        public async Task<IActionResult> Remove([FromBody] UserRequest Request, [FromServices] IConfiguration Config)
        {
            string ConnectionString = Config.GetConnectionString("DefaultConnection");
            using NpgsqlConnection Connection = new NpgsqlConnection(ConnectionString);

            string Token = Request.Token;
            string HashToken = Hashing.Hash(Token);

            // DELETE FROM users WHERE access_token = HashToken
            await Connection.ExecuteAsync(SQLStatements.RemoveUser, new { Token = HashToken });

            return Ok();
        }
    }
}
