using Microsoft.Data.Sqlite;
using Drustvena_mreza_Clanovi_i_grupe.Models;
using System.Collections.Generic;

namespace Drustvena_mreza_Clanovi_i_grupe.Repositories
{
    public class PostDbRepository
    {
        private readonly string connectionString;

        public PostDbRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionString:SQLiteConnection"]
                    ?? throw new InvalidOperationException("Missing connection string!");

        }

        public List<Post> GetAllPostsWithUsernames()
        {
            List<Post> posts = new List<Post>();

            SqliteConnection connection = new SqliteConnection(connectionString);
            connection.Open();

            string query = @"
                SELECT Posts.Id, Posts.Content, Posts.Date, Users.Username, Posts.UserId
                FROM Posts
                LEFT JOIN Users ON Posts.UserId = Users.Id";

            SqliteCommand command = new SqliteCommand(query, connection);
            SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Post post = new Post
                {
                    Id = reader.GetInt32(0),
                    Content = reader.GetString(1),
                    Date = reader.GetString(2),
                    Username = reader.IsDBNull(3) ? "Ismeretlen" : reader.GetString(3),
                    UserId = reader.IsDBNull(4) ? null : reader.GetInt32(4)
                };

                posts.Add(post);
            }

            reader.Close();
            connection.Close();

            return posts;
        }
    }
}
