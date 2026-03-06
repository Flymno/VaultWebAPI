using VaultWebAPI.Models;
using Npgsql;
using VaultWebAPI.Services;
using Dapper;
using VaultWebAPI.Data.Queries;
using VaultWebAPI.Exceptions;

namespace VaultWebAPI.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        private readonly IHashService _hashService;

        public UserRepository(IConfiguration config, IHashService hashService)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
            _hashService = hashService;
        }

        public async Task<User> GetByTokenAsync(string token)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            string hashToken = _hashService.GetAuthHash(token);

            User? user = await connection.QueryFirstOrDefaultAsync<User>(SQLStatements.GetUser, new { Token = hashToken });
            if (user == null) throw new NotFoundVaultException("The user could not be found");

            return user with { AccessToken = token };
        }

        public async Task<string> CreateUser()
        {
            using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);

            string token = Guid.NewGuid().ToString();
            string hashToken = _hashService.GetAuthHash(token);

            await connection.ExecuteAsync(SQLStatements.RegisterUser, new { Token = hashToken });

            return token;
        }

        public async Task<int> RemoveUserAsync(int userId)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);

            int rowsAffected = await connection.ExecuteAsync(SQLStatements.RemoveUser, new { UserId = userId });
            if (rowsAffected == 0) throw new NotFoundVaultException("The user could not be found or was already removed");
            return rowsAffected;
        }
    }
}
