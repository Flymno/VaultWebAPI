using System.Text;
using System.Security.Cryptography;

namespace VaultWebAPI.Utility
{
    public class Hashing
    {
        public static string Hash(string Plain)
        {
            byte[] InBytes = Encoding.UTF8.GetBytes(Plain);
            byte[] HashBytes = SHA256.HashData(InBytes);
            string HashResult = Convert.ToHexString(HashBytes);

            return HashResult;
        }
    }
}
