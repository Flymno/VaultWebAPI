using VaultWebAPI.DTOs;
using VaultWebAPI.Models;

namespace VaultWebAPI.Services
{
    public class TreeService : ITreeService
    {
        public List<NodeTreeDTO> BuildNodeTree(List<Node> flatNodes)
        {
            Dictionary<int, NodeTreeDTO> treeMap = flatNodes.Select(r => new NodeTreeDTO
            (
                r.NodeId,
                r.ParentId,
                r.IsCategory,
                r.Name,
                r.Content,
                r.DateCreated,
                r.LastModified,
                new List<NodeTreeDTO>()
            )).ToDictionary(t => t.NodeId);

            List<NodeTreeDTO> nodeTreeRoots = new List<NodeTreeDTO>();

            foreach (NodeTreeDTO node in treeMap.Values)
            {
                if (node.ParentId.HasValue && treeMap.TryGetValue(node.ParentId.Value, out NodeTreeDTO? parent))
                {
                    parent.Children.Add(node);
                } else
                {
                    nodeTreeRoots.Add(node);
                }
            }

            return nodeTreeRoots;
        }
    }
}
