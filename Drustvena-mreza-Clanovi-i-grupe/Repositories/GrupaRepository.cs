using Drustvena_mreza_Clanovi_i_grupe.Models;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Drustvena_mreza_Clanovi_i_grupe.Repositories
{
    public class GrupaRepository
    {
        private readonly string _connectionString;

        public GrupaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SQLiteConnection")!;
        }

        public Grupa? GetById(int grupaId)
        {
            try
            {
                using (SqliteConnection connection = new SqliteConnection(_connectionString))
                {
                    connection.Open();

                    SqliteCommand command = connection.CreateCommand();
                    command.CommandText = @"
                        SELECT Id, Ime, DatumOsnivanja FROM Grupe WHERE Id = @grupaId";
                    command.Parameters.AddWithValue("@grupaId", grupaId);

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Grupa grupa = new Grupa(
                                reader.GetInt32(0),   // Id
                                reader.GetString(1),  // Ime
                                reader.GetDateTime(2) // DatumOsnivanja
                            );
                            grupa.Korisnici = GetClanovi(grupaId, connection);
                            return grupa;
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Database error in GetById", ex);
            }
        }

        private List<Korisnik> GetClanovi(int grupaId, SqliteConnection connection)
        {
            List<Korisnik> clanovi = new List<Korisnik>();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"
                SELECT k.Id, k.Username, k.Name, k.Surname, k.Birthday
                FROM Users k
                JOIN GrupaKorisnici gk ON k.Id = gk.KorisnikId
                WHERE gk.GrupaId = @grupaId";

            command.Parameters.AddWithValue("@grupaId", grupaId);

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
                    clanovi.Add(korisnik);
                }
            }
            return clanovi;
        }
    }
}
