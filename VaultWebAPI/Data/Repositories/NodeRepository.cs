using Dapper;
using Npgsql;
using System.Xml.Linq;
using VaultWebAPI.Data.Queries;
using VaultWebAPI.Exceptions;
using VaultWebAPI.Models;

namespace VaultWebAPI.Data.Repositories
{
    public class NodeRepository : INodeRepository
    {
        private readonly string _connectionString;

        public NodeRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public async Task<Node> CreateNodeAsync(int userId, int? parentId, string name, bool isCategory)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);

            Node newNode = await connection.QuerySingleAsync<Node>(SQLStatements.CreateNode, new { UserId = userId, ParentId = parentId, IsCategory = isCategory, Name = name });
            return newNode;
        }

        public async Task<List<Node>> GetAllUserNodesAsync(int userId)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);

            IEnumerable<Node> userNodes = await connection.QueryAsync<Node>(SQLStatements.GetUserNodes, new { UserId = userId});
            return userNodes.ToList();
        }

        public async Task DeleteNodeAsync(int userId, int nodeId)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);

            int rowsAffected = await connection.ExecuteAsync(SQLStatements.DeleteNode, new { UserId = userId, NodeId = nodeId });
            if (rowsAffected == 0) throw new NotFoundVaultException($"Node: {nodeId} not found or unauthorized");
        }
    }
}
