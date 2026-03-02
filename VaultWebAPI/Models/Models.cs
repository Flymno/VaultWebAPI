namespace VaultWebAPI.Models
{
    public record User(int UserId, string AccessToken);

    public record Node(int NodeId, int UserId, int? ParentId, bool IsCategory, string Name, string? Content, DateTime DateCreated, DateTime LastModified);
}
