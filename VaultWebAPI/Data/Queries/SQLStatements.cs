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
            WHERE user_id = @UserId
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
            INSERT INTO nodes
            (user_id, parent_id, is_category, name)
            SELECT @UserId, @ParentId, @IsCategory, @Name
            WHERE (@ParentId IS NULL OR EXISTS (
                SELECT 1 FROM nodes 
                WHERE node_id = @ParentId 
                AND user_id = @UserId
            ))
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

        public static string UpdateNode =>
            """
            UPDATE nodes
            SET 
            parent_id = @ParentId, 
            name = @Name,
            content = @Content, 
            last_modified = @LastModified
            WHERE 
            node_id = @NodeId
            AND user_id = @UserId
            AND (@ParentId IS NULL OR EXISTS (
                SELECT 1 FROM nodes WHERE node_id = @ParentId AND user_id = @UserId
            ))
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

        public static string DeleteNode =>
            """
            DELETE FROM nodes
            WHERE user_id = @UserId AND node_id = @NodeId
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
