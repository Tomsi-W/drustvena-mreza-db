using Drustvena_mreza_Clanovi_i_grupe.Models;
using Microsoft.Data.Sqlite;

namespace Drustvena_mreza_Clanovi_i_grupe.Repositories
{
    public class GrupaDbRepository
    {
        public List<Grupa> GetAll()
        {
            List<Grupa> grupe = new List<Grupa>();
            try
            {
                using SqliteConnection connection = new SqliteConnection("Data Source=DataBase/socialnetwork.db");
                connection.Open();

                string query = "SELECT Id, Name, CreationDate FROM Groups";
                using SqliteCommand command = new SqliteCommand(query, connection);

                using SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    grupe.Add(new Grupa(
                        Convert.ToInt32(reader["Id"]),
                        reader["Name"].ToString(),
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
                using SqliteConnection connection = new SqliteConnection("Data Source=DataBase/socialnetwork.db");
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
                return null;
                Console.WriteLine($"Neocekivana greska: {ex.Message}");
            }
        }
    }
}
