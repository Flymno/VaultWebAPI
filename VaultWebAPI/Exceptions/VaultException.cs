namespace VaultWebAPI.Exceptions
{
    public abstract class VaultException : Exception
    {
        protected VaultException(string message) : base(message) { }
    }
}
