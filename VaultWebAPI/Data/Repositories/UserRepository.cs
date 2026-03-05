using VaultWebAPI.Models;
using Npgsql;
using VaultWebAPI.Services;
using Dapper;
using VaultWebAPI.Data.Queries;

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

        public async Task<User?> GetByTokenAsync(string token)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            string hashToken = _hashService.Hash(token);

            User? user = await connection.QueryFirstOrDefaultAsync<User>(SQLStatements.GetUser, new { Token = hashToken });
            if (user == null) { return null; }

            return user with { AccessToken = token };
        }

        public async Task<string?> CreateUser()
        {
            using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);

            string token = Guid.NewGuid().ToString();
            string hashToken = _hashService.Hash(token);

            try { await connection.ExecuteAsync(SQLStatements.RegisterUser, new { Token = hashToken }); }
            catch { return null; }

            return token;
        }

        public async Task<int?> RemoveUserAsync(int userId)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);

            try
            { 
                int rowsAffected = await connection.ExecuteAsync(SQLStatements.RemoveUser, new { UserId = userId });
                return rowsAffected > 0 ? userId : null;
            } 
            catch { return null; }
        }
    }
}
