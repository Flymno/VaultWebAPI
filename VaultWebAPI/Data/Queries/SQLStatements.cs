namespace VaultWebAPI.Data.Queries
{
    public class SQLStatements
    {
        public static string RegisterUser =>
            """
            INSERT INTO users
            (access_token)
            VALUES
            (@Token)
            """;

        public static string RemoveUser =>
            """
            DELETE FROM users
            WHERE access_token = @Token
            """;

        public static string GetUser =>
            """
            SELECT user_id AS UserId,
            access_token AS AccessToken
            FROM users
            WHERE access_token = @Token
            """;

        public static string CreateNode =>
            """
            INSERT INTO Nodes
            (user_id, parent_id, is_category, name)
            VALUES
            (@UserId, @ParentId, @IsCategory, @Name)
            RETURNING 
            node_id AS NodeId,
            user_id AS UserId,
            parent_id AS ParentId,
            is_category AS IsCategory,
            name AS Name,
            content AS Content,
            date_created AS DateCreated,
            last_modified AS LastModified
            """;

        public static string GetUserNodes =>
            """
            SELECT
            node_id AS NodeId,
            user_id AS UserId,
            parent_id AS ParentId,
            is_category AS IsCategory,
            name AS Name,
            content AS Content,
            date_created AS DateCreated,
            last_modified AS LastModified
            FROM nodes
            WHERE user_id = @UserId
            """;
    }
}
