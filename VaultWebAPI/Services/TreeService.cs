using VaultWebAPI.DTOs;
using VaultWebAPI.Models;

namespace VaultWebAPI.Services
{
    public class TreeService : ITreeService
    {
        public List<NodeResponse.NodeTreeDTO> BuildNodeTree(List<Node> flatNodes)
        {
            Dictionary<int, NodeResponse.NodeTreeDTO> treeMap = flatNodes.Select(r => new NodeResponse.NodeTreeDTO
            (
                r.NodeId,
                r.ParentId,
                r.IsCategory,
                r.Name,
                r.Content,
                r.DateCreated,
                r.LastModified,
                new List<NodeResponse.NodeTreeDTO>()
            )).ToDictionary(t => t.NodeId);

            List<NodeResponse.NodeTreeDTO> nodeTreeRoots = new List<NodeResponse.NodeTreeDTO>();

            foreach (NodeResponse.NodeTreeDTO node in treeMap.Values)
            {
                if (node.ParentId.HasValue && treeMap.TryGetValue(node.ParentId.Value, out NodeResponse.NodeTreeDTO? parent))
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
