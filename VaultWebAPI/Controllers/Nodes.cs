using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Runtime.CompilerServices;
using VaultWebAPI.Data.Queries;
using VaultWebAPI.Utility;

namespace VaultWebAPI.Controllers
{
    [Route("api/nodes")]
    [ApiController]
    public class Nodes : ControllerBase
    {
        public class CreateRequest { public string Token { get; set; } public string Name { get; set; } }
        public class QueryResultUser { public string user_id { get; set; } public string access_token { get; set; }}

        [HttpPost("create")]
        public async Task<IActionResult> CreateNode([FromBody] CreateRequest Request, [FromServices] IConfiguration Config)
        {
            string ConnectionString = Config.GetConnectionString("DefaultConnection");
            using NpgsqlConnection Connection = new NpgsqlConnection(ConnectionString);

            string HashToken = Hashing.Hash(Request.Token);

            QueryResultUser CurrentUser;
            try
            {
                 CurrentUser = Connection.Query<QueryResultUser>(SQLStatements.GetUser, new { Token = HashToken }).ToList()[0];
            } catch
            {
                return Ok();
            }

            Console.WriteLine(CurrentUser.user_id);

            return Ok();
        }
    }
}
