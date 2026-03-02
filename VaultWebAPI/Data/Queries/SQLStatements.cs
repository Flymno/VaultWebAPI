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

        public static string RemoveUser =>
            """
            DELETE FROM users
            WHERE access_token = @Token
            """;
        public static string GetUser =>
            """
            SELECT user_id, access_token
            FROM users
            WHERE access_token = @Token
            """;
    }
}
