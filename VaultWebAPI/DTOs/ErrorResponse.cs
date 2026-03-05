namespace VaultWebAPI.DTOs
{
    public record ErrorResponseDTO(
        int Status,
        string Title,
        string Detail
    );
}
