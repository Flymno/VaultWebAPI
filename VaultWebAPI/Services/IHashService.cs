namespace VaultWebAPI.Services
{
    public interface IHashService
    {
        string GetAuthHash(string rawToken);

        byte[] GetEncryptionKey(string rawToken);
    }
}
