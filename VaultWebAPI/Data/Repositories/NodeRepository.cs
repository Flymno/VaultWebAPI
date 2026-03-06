using Dapper;
using Npgsql;
using System.Data;
using System.Net;
using System.Xml.Linq;
using VaultWebAPI.Data.Queries;
using VaultWebAPI.Exceptions;
using VaultWebAPI.Models;
using VaultWebAPI.Services;

namespace VaultWebAPI.Data.Repositories
{
    public class NodeRepository : INodeRepository
    {
        private readonly string _connectionString;
        private readonly IEncryptionService _encryptionService;

        public NodeRepository(IConfiguration config, IEncryptionService encryptionService)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
            _encryptionService = encryptionService;
        }

        public async Task<Node> CreateNodeAsync(int userId, int? parentId, string name, bool isCategory)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);

            Node? newNode = await connection.QuerySingleOrDefaultAsync<Node>(SQLStatements.CreateNode, new { UserId = userId, ParentId = parentId, IsCategory = isCategory, Name = name });
            if (newNode == null) throw new NotFoundVaultException("Parent not found or access denied");
            return newNode;
        }

        public async Task<Node> UpdateNodeAsync(int userId, int nodeId, int? parentId, string name, string? content)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);

            string? encryptedContent = _encryptionService.Encrypt(content);

            Node? updatedNode = await connection.QuerySingleOrDefaultAsync<Node>(SQLStatements.UpdateNode, new { NodeId = nodeId, UserId = userId, ParentId = parentId, Name = name, Content = encryptedContent });
            if (updatedNode == null) throw new NotFoundVaultException("Node not found or access denied");

            return updatedNode with { Content = _encryptionService.Decrypt(updatedNode.Content)};
        }

        public async Task<List<Node>> GetAllUserNodesAsync(int userId)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);

            IEnumerable<Node> userNodes = await connection.QueryAsync<Node>(SQLStatements.GetUserNodes, new { UserId = userId});

            return userNodes.Select(node => node with { Content = _encryptionService.Decrypt(node.Content) }).ToList();
        }

        public async Task DeleteNodeAsync(int userId, int nodeId)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);

            int rowsAffected = await connection.ExecuteAsync(SQLStatements.DeleteNode, new { UserId = userId, NodeId = nodeId });
            if (rowsAffected == 0) throw new NotFoundVaultException($"Node: {nodeId} not found or unauthorized");
        }
    }
}
