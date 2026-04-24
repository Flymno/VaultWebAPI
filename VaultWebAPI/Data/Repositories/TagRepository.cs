using Dapper;
using Npgsql;
using VaultWebAPI.Data.Queries;
using VaultWebAPI.Exceptions;
using VaultWebAPI.Models;

namespace VaultWebAPI.Data.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly string _connectionString;

        public TagRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public async Task<Tag> CreateTagAsync(int userId, string name, string color)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            Tag? newTag = await connection.QuerySingleOrDefaultAsync<Tag>(SQLStatements.CreateTag, new { UserId = userId, Name = name, Color = color });
            if (newTag == null) throw new NotFoundVaultException("Something Went Wrong");

            return newTag;
        }
    }
}
