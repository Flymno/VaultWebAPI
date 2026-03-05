namespace VaultWebAPI.Exceptions
{
    public class NotFoundVaultException : VaultException
    {
        public NotFoundVaultException(string message = "The resource was not found") : base(message) { }
    }
}
