using Microsoft.Data.Sqlite;

namespace Drustvena_mreza_Clanovi_i_grupe.Repositories
{
    public class GroupMembershipDbRepository
    {
        private readonly string connectionString;

        public GroupMembershipDbRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionString:SQLiteConnection"];
        }

        public int AddUserToGroup(int userId, int groupId)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "INSERT INTO GroupMemberships (UserId, GroupId) VALUES (@UserId, @GroupId)";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@GroupId", groupId);

                return command.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri konekciji ili izvršavanju neispravnih SQL upita: {ex.Message}");
                throw;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
                throw;
            }
        }

        public int RemoveUserFromGroup(int userId, int groupId)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "DELETE FROM GroupMemberships WHERE UserId = @UserId AND GroupId = @GroupId";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@GroupId", groupId);

                return command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Greska prilikom izbacivanja korisnika iz grupe: {e.Message}");
                throw;
            }
        }
    }
}
