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
            _connectionString = configuration["ConnectionString:SQLiteConnection"];
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
                                KorisnickoIme = reader.GetString(1),
                                Ime = reader.GetString(2),
                                Prezime = reader.GetString(3),
                                DatumRodjenja = DateTime.Parse(reader.GetString(4))
                            };
                            korisnici.Add(korisnik);
                        }
                    }
                }
                return korisnici;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetAll greška: " + ex.Message);
                throw;
            }

        }

        

        public Korisnik? GetById(int id)
        {
            try
            {
                using (SqliteConnection connection = new SqliteConnection(_connectionString))
                {
                    connection.Open();
                    SqliteCommand command = connection.CreateCommand();
                    command.CommandText = @"
                        SELECT Id, Username, Name, Surname, Birthday
                        FROM Users
                        WHERE Id = @id";

                    command.Parameters.AddWithValue("@id", id);

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Korisnik
                            {
                                Id = reader.GetInt32(0),
                                KorisnickoIme = reader.GetString(1),
                                Ime = reader.GetString(2),
                                Prezime = reader.GetString(3),
                                DatumRodjenja = reader.GetDateTime(4)
                            };
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

        public Korisnik Create(Korisnik korisnik)
        {
            try
            {
                using (SqliteConnection connection = new SqliteConnection(_connectionString))
                {
                    connection.Open();
                    SqliteCommand command = connection.CreateCommand();
                    command.CommandText = @"
                        INSERT INTO Users (Username, Name, Surname, Birthday)
                        VALUES (@Username, @Name, @Surname, @Birthday);
                        SELECT last_insert_rowid();";

                    command.Parameters.AddWithValue("@Username", korisnik.KorisnickoIme);
                    command.Parameters.AddWithValue("@Name", korisnik.Ime);
                    command.Parameters.AddWithValue("@Surname", korisnik.Prezime);
                    command.Parameters.AddWithValue("@Birthday", korisnik.DatumRodjenja);

                    korisnik.Id = Convert.ToInt32(command.ExecuteScalar());
                }
                return korisnik;
            }
            catch (Exception ex)
            {
                throw new Exception("Database error in Create", ex);
            }
        }

        public bool Update(int id, Korisnik korisnik)
        {
            try
            {
                using (SqliteConnection connection = new SqliteConnection(_connectionString))
                {
                    connection.Open();
                    SqliteCommand command = connection.CreateCommand();
                    command.CommandText = @"
                        UPDATE Users
                        SET Username = @Username, Name = @Name, Surname = @Surname, Birthday = @Birthday
                        WHERE Id = @id";

                    command.Parameters.AddWithValue("@Username", korisnik.KorisnickoIme);
                    command.Parameters.AddWithValue("@Name", korisnik.Ime);
                    command.Parameters.AddWithValue("@Surname", korisnik.Prezime);
                    command.Parameters.AddWithValue("@Birthday", korisnik.DatumRodjenja);
                    command.Parameters.AddWithValue("@id", id);

                    int affectedRows = command.ExecuteNonQuery();
                    return affectedRows > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Database error in Update", ex);
            }
        }

        public bool Delete(int id)
        {
            try
            {
                using (SqliteConnection connection = new SqliteConnection(_connectionString))
                {
                    connection.Open();
                    SqliteCommand command = connection.CreateCommand();
                    command.CommandText = "DELETE FROM Users WHERE Id = @id";

                    command.Parameters.AddWithValue("@id", id);

                    int affectedRows = command.ExecuteNonQuery();
                    return affectedRows > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Database error in Delete", ex);
            }
        }
    }
}
