namespace VaultWebAPI.DTOs
{
    public static class NodeRequest
    {
        public record NodeCreateRequestDTO(int? ParentId, string Name, bool IsCategory);
    }
}
