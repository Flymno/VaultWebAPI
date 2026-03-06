using System.Text;
using System.Security.Cryptography;

namespace VaultWebAPI.Services
{
    public class HashService : IHashService
    {
        public string GetAuthHash(string rawToken)
        {
            byte[] InBytes = Encoding.UTF8.GetBytes(rawToken);
            byte[] HashBytes = SHA512.HashData(InBytes);
            
            return Convert.ToHexString(HashBytes);
        }

        public byte[] GetEncryptionKey(string rawToken)
        {
            byte[] InBytes = Encoding.UTF8.GetBytes(rawToken);
            return SHA256.HashData(InBytes);
        }
    }
}
