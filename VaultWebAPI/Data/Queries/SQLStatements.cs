namespace VaultWebAPI.Data.Queries
{
    public class SQLStatements
    {
        public static string RegisterUser =>
            """
            INSERT INTO users
            VALUES
            (DEFAULT, @Token)
            """;
    }
}
