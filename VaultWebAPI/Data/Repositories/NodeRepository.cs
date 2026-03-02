using Dapper;
using Npgsql;
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

        public async Task<Node?> CreateNodeAsync(int userId, int? parentId, string name)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);

            bool isCategory = (parentId == null);

            try 
            {
                Node newNode = await connection.QuerySingleAsync<Node>(SQLStatements.CreateNode, new { UserId = userId, ParentId = parentId, IsCategory = isCategory, Name = name });
                return newNode;
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); return null; }
        }
    }
}
