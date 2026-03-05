using VaultWebAPI.DTOs;
using VaultWebAPI.Models;

namespace VaultWebAPI.Services
{
    public interface ITreeService
    {
        List<NodeTreeDTO> BuildNodeTree(List<Node> flatNodes);
    }
}
