using Drustvena_mreza_Clanovi_i_grupe.Models;
using Microsoft.Data.Sqlite;

namespace Drustvena_mreza_Clanovi_i_grupe.Repositories
{
    public class KorisnikDbRepository
    {
        private readonly string _connectionString = "Data Source=DataBase/socialnetwork.db";

        public List<Korisnik> GetAll()
        {
            List<Korisnik> korisnici = new();
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();

                string query = "SELECT Id, Username, Name, Surname, Birthday FROM Users";
                using var command = new SqliteCommand(query, connection);
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    korisnici.Add(new Korisnik(
                        reader.GetInt32(0),                              // Id
                        reader.GetString(1),                             // Username
                        reader.GetString(2),                             // Name
                        reader.GetString(3),                             // Surname
                        DateTime.Parse(reader.GetString(4))              // Birthday
                    ));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Greška: " + e.Message);
            }

            return korisnici;
        }

        public Korisnik? GetById(int id)
        {
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();

                string query = "SELECT Id, Username, Name, Surname, Birthday FROM Users WHERE Id = @id";
                using var command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new Korisnik(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                        DateTime.Parse(reader.GetString(4))
                    );
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Greška: " + e.Message);
            }

            return null;
        }
    }
}
