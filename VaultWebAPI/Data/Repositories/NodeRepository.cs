using Dapper;
using Npgsql;
using System.Xml.Linq;
using VaultWebAPI.Data.Queries;
using VaultWebAPI.Models;

namespace VaultWebAPI.Data.Repositories
{
    public class NodeRepository
    {
        private readonly string _connectionString;

        public NodeRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public async Task<Node?> CreateNodeAsync(int userId, int? parentId, string name, bool isCategory)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);

            try 
            {
                Node newNode = await connection.QuerySingleAsync<Node>(SQLStatements.CreateNode, new { UserId = userId, ParentId = parentId, IsCategory = isCategory, Name = name });
                return newNode;
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); return null; }
        }

        public async Task<List<Node>?> GetAllUserNodesAsync(int userId)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);

            try
            {
                IEnumerable<Node>? userNodes = await connection.QueryAsync<Node>(SQLStatements.GetUserNodes, new { UserId = userId});
                return userNodes.ToList();
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); return null; }

        }
    }
}
