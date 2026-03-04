namespace VaultWebAPI.DTOs
{
    public class NodeResponse
    {
        public record NodeTreeDTO(
            int NodeId,
            int? ParentId,
            bool IsCategory,
            string Name,
            string? Content,
            DateTime DateCreated,
            DateTime LastModified,
            List<NodeTreeDTO> Children
            );
    }
}
