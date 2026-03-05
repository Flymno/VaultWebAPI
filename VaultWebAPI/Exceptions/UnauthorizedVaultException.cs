namespace VaultWebAPI.Exceptions
{
    public class UnauthorizedVaultException : VaultException
    {
        public UnauthorizedVaultException(string message = "Invalid Access Token") : base(message) { }
    }
}
