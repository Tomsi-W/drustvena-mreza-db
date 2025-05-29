using Drustvena_mreza_Clanovi_i_grupe.Models;
using Microsoft.Data.Sqlite;

namespace Drustvena_mreza_Clanovi_i_grupe.Repositories
{
    public class GrupaDbRepository
    {
        private readonly string connectionString;

        public GrupaDbRepository (IConfiguration configuration)
        {
            connectionString = configuration["ConnectionString:SQLiteConnection"];
        }

        public List<Grupa> GetPaged(int page, int pageSize)
        {
            List<Grupa> grupe = new List<Grupa>();
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "SELECT Id, Name, CreationDate FROM Groups LIMIT @PageSize OFFSET @Offset";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@PageSize", pageSize);
                command.Parameters.AddWithValue("@Offset", pageSize * (page - 1));

                using SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    grupe.Add(new Grupa(
                        Convert.ToInt32(reader["Id"]),
                        Convert.ToString(reader["Name"]),
                        DateTime.Parse(reader["CreationDate"].ToString())
                        ));
                }
            }
            catch (SqliteException e)
            {

                Console.WriteLine($"Greska pri radu sa bazom: {e.Message}");
            }
            catch (FormatException e)
            {
                Console.WriteLine($"Greska u konverziji podataka: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Neocekivana greska: {e.Message}");
            }

            return grupe;
        }

        public Grupa? GetById(int grupaId)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "SELECT Id, Name, CreationDate FROM Groups WHERE Id = @id";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@id", grupaId);

                using SqliteDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    int id = Convert.ToInt32(reader["Id"]);
                    string ime = reader["Name"].ToString();
                    DateTime datumOsnivanja = DateTime.Parse(reader["CreationDate"].ToString());
                    Grupa grupa = new Grupa(id, ime, datumOsnivanja);
                    return grupa;
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neocekivana greska: {ex.Message}");
                return null;
            }
        }
        public Grupa Create(Grupa grupa)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "INSERT INTO Groups (Name,CreationDate) VALUES (@Ime, @DatumOsnivanja); SELECT last_insert_rowid();";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Ime", grupa.Ime);
                command.Parameters.AddWithValue("@DatumOsnivanja", grupa.DatumOsnivanja.ToString("yyyy-MM-dd"));

                grupa.Id = Convert.ToInt32(command.ExecuteScalar());
                return grupa;
            }
            catch(Exception e)
            {
                Console.WriteLine("Greska pri dodavanju grupe:" + e.Message);
                throw;
            }
        }

        public int Update( int grupaId, Grupa grupa)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "UPDATE Groups SET Name = @Ime, CreationDate = @DatumOsnivanja WHERE Id = @Id";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Id", grupaId);
                command.Parameters.AddWithValue("@Ime", grupa.Ime);
                command.Parameters.AddWithValue("@DatumOsnivanja", grupa.DatumOsnivanja);

                return command.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greska pri izvrsavanju SQL upita: {ex.Message}");
                throw;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greska u konverziji datuma: {ex.Message}");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Greska u radu sa konekcijom: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neocekivana greska: {ex.Message}");
                throw;
            }

        }

        public int Delete(int grupaId )
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "DELETE FROM Groups WHERE Id = @Id";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Id", grupaId);

                return command.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greska pri brisanju grupe: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neosekivana greska: {ex.Message}");
                throw;
            }
        }

        public int CountAll()
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "SELECT COUNT(*) AS Count FROM Groups";
                using SqliteCommand command = new SqliteCommand(query, connection);
                int totalCount = Convert.ToInt32(command.ExecuteScalar());
                return totalCount;
            }
            catch (SqliteException ex)
            {
                Console.WriteLine("Greska prilikom brojanja grupa:" + ex.Message);
                throw;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greska u konverziji podataka iz baze: {ex.Message}");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena vise puta: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neocekivana greska: {ex.Message}");
                throw;
            }
        }
    }
}
