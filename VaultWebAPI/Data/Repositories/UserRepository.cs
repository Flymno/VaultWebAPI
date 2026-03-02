using VaultWebAPI.Models;
using Npgsql;
using VaultWebAPI.Utility;
using Dapper;
using VaultWebAPI.Data.Queries;

namespace VaultWebAPI.Data.Repositories
{
    public class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public async Task<User?> GetByTokenAsync(string token)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            string hashToken = Hashing.Hash(token);

            User? user = await connection.QueryFirstOrDefaultAsync<User>(SQLStatements.GetUser, new { Token = hashToken });
            if (user == null) { return null; }

            return user with { AccessToken = token };
        }

        public async Task<string?> CreateUser()
        {
            using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);

            string token = Guid.NewGuid().ToString();
            string hashToken = Hashing.Hash(token);

            try { await connection.ExecuteAsync(SQLStatements.RegisterUser, new { Token = hashToken }); }
            catch { return null; }

            return token;
        }

        public async Task<bool> RemoveUserAsync(string token)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            string hashToken = Hashing.Hash(token);

            try
            { 
                int rowsAffected = await connection.ExecuteAsync(SQLStatements.RemoveUser, new { Token = hashToken }); 
                return rowsAffected > 0;
            } 
            catch { return false; }
        }
    }
}
