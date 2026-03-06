namespace VaultWebAPI.DTOs
{
    public record NodeCreateRequestDTO(int? ParentId, string Name, bool IsCategory);

    public record NodeUpdateRequestDTO(int? ParentId, string Name, string? Content);
}
