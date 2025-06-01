using Drustvena_mreza_Clanovi_i_grupe.Models;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Drustvena_mreza_Clanovi_i_grupe.Repositories
{
    public class KorisnikRepository
    {
        private readonly string _connectionString;

        public KorisnikRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SQLiteConnection")!;
        }

        public List<Korisnik> GetAll(int page, int pageSize)
        {
            try
            {
                if (page < 1 || pageSize < 1)
                {
                    throw new ArgumentException("Page and pageSize must be greater than 0.");
                }

                List<Korisnik> korisnici = new List<Korisnik>();
                using (SqliteConnection connection = new SqliteConnection(_connectionString))
                {
                    connection.Open();

                    SqliteCommand command = connection.CreateCommand();
                    command.CommandText = @"
                        SELECT Id, Username, Name, Surname, Birthday
                        FROM Users
                        LIMIT @pageSize OFFSET @offset";

                    command.Parameters.AddWithValue("@pageSize", pageSize);
                    command.Parameters.AddWithValue("@offset", (page - 1) * pageSize);

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Korisnik korisnik = new Korisnik
                            {
                                Id = reader.GetInt32(0),
                                KorisnickoIme = reader.GetString(1), // Username
                                Ime = reader.GetString(2),           // Name
                                Prezime = reader.GetString(3),       // Surname
                                DatumRodjenja = reader.GetDateTime(4) // Birthday
                            };
                            korisnici.Add(korisnik);
                        }
                    }
                }
                return korisnici;
            }
            catch (Exception ex)
            {
                throw new Exception("Database error in GetAll", ex);
            }
        }
    }
}
