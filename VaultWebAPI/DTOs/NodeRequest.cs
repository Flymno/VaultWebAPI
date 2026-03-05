namespace VaultWebAPI.DTOs
{
    public record NodeCreateRequestDTO(int? ParentId, string Name, bool IsCategory);
}
