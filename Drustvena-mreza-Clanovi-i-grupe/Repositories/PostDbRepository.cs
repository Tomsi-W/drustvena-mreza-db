using Microsoft.Data.Sqlite;
using Drustvena_mreza_Clanovi_i_grupe.Models;

namespace Drustvena_mreza_Clanovi_i_grupe.Repositories
{
	public class PostDbRepository
	{
		private readonly string connectionString;

		public PostDbRepository(IConfiguration configuration)
		{
			connectionString = configuration["ConnectionString:SQLiteConnection"];
		}

		public List<Post> GetAllPostsWithUsernames()
		{
			var posts = new List<Post>();

			using var connection = new SqliteConnection(connectionString);
			connection.Open();

			string query = @"
                SELECT Posts.Id, Posts.Content, Posts.Date, Users.Username, Posts.UserId
                FROM Posts
                LEFT JOIN Users ON Posts.UserId = Users.Id";

			using var command = new SqliteCommand(query, connection);
			using var reader = command.ExecuteReader();

			while (reader.Read())
			{
				posts.Add(new Post
				{
					Id = reader.GetInt32(0),
					Content = reader.GetString(1),
					Date = reader.GetString(2),
					Username = reader.IsDBNull(3) ? "Nepoznato" : reader.GetString(3),
					UserId = reader.IsDBNull(4) ? null : reader.GetInt32(4)
				});
			}

			return posts;
		}
	}
}
