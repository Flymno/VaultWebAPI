using VaultWebAPI.DTOs;
using VaultWebAPI.Models;

namespace VaultWebAPI.Services
{
    public interface ITreeService
    {
        List<NodeResponse.NodeTreeDTO> BuildNodeTree(List<Node> flatNodes);
    }
}
