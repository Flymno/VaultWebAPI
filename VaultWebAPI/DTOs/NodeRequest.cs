namespace VaultWebAPI.DTOs
{
    public static class NodeRequest
    {
        public record NodeCreateRequest(string Token, int? ParentId, string Name);
    }
}
