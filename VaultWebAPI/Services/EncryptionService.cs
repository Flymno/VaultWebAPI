using System.Security.Cryptography;
using System.Text;

namespace VaultWebAPI.Services
{
    public class EncryptionService : IEncryptionService
    {
        private readonly IAuthService _authService;
        private readonly IHashService _hashService;

        public EncryptionService(IAuthService authService, IHashService hashService)
        {
            _authService = authService;
            _hashService = hashService;
        }

        private byte[] GetAesKey()
        {
            string rawToken = _authService.GetRawToken();

            return _hashService.GetEncryptionKey(rawToken);
        }
        
        public string? Encrypt(string? plainText)
        {
            if (string.IsNullOrEmpty(plainText)) return plainText;

            using Aes aes = Aes.Create();
            aes.Key = GetAesKey();
            aes.GenerateIV();

            using ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            byte[] combined = new byte[aes.IV.Length + cipherBytes.Length];
            Buffer.BlockCopy(aes.IV, 0, combined, 0, aes.IV.Length);
            Buffer.BlockCopy(cipherBytes, 0, combined, aes.IV.Length, cipherBytes.Length);

            return Convert.ToBase64String(combined);
        }

        public string? Decrypt(string? cipherText)
        {
            if (string.IsNullOrEmpty(cipherText)) return cipherText;

            byte[] combined = Convert.FromBase64String(cipherText);
            using Aes aes = Aes.Create();
            aes.Key = GetAesKey();

            byte[] iv = new byte[16];
            byte[] actualCipher = new byte[combined.Length - 16];

            Buffer.BlockCopy(combined, 0, iv, 0, 16);
            Buffer.BlockCopy(combined, 16, actualCipher, 0, combined.Length - 16);

            aes.IV = iv;

            using ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] plainBytes = decryptor.TransformFinalBlock(actualCipher, 0, actualCipher.Length);

            return Encoding.UTF8.GetString(plainBytes);
        }

    }
}
