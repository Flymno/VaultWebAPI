using VaultWebAPI.Models;

namespace VaultWebAPI.Data.Repositories
{
    public interface INodeRepository
    {
        Task<Node> CreateNodeAsync(int userId, int? parentId, string name, bool isCategory);
        Task<List<Node>> GetAllUserNodesAsync(int userId);
        Task DeleteNodeAsync(int userId, int nodeId);
    }
}
