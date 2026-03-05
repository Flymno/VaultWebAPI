using System.Text;
using System.Security.Cryptography;

namespace VaultWebAPI.Services
{
    public class HashService : IHashService
    {
        public string Hash(string Plain)
        {
            byte[] InBytes = Encoding.UTF8.GetBytes(Plain);
            byte[] HashBytes = SHA256.HashData(InBytes);
            string HashResult = Convert.ToHexString(HashBytes);

            return HashResult;
        }
    }
}
