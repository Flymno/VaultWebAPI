namespace VaultWebAPI.DTOs
{
    public static class NodeRequest
    {
        public record NodeCreateRequestDTO(string Token, int? ParentId, string Name);
    }
}
