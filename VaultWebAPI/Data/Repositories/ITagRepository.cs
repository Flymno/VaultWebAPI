using VaultWebAPI.Models;

namespace VaultWebAPI.Data.Repositories
{
    public interface ITagRepository
    {
        Task<Tag> CreateTagAsync(int userId, string name, string color)
    }
}
