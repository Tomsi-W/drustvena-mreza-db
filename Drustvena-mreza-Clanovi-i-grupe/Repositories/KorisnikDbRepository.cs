using Drustvena_mreza_Clanovi_i_grupe.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace Drustvena_mreza_Clanovi_i_grupe.Repositories
{
    public class KorisnikDbRepository
    {
        private readonly string _connectionString = "Data Source=DataBase/socialnetwork.db";

        public List<Korisnik> GetAll()
        {
            List<Korisnik> korisnici = new List<Korisnik>();
            try
            {
                SqliteConnection connection = new SqliteConnection(_connectionString);
                connection.Open();

                string query = "SELECT Id, Username, Name, Surname, Birthday FROM Users";
                SqliteCommand command = new SqliteCommand(query, connection);
                SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Korisnik korisnik = new Korisnik(
                        reader.GetInt32(0), // Id
                        reader.GetString(1), // Username
                        reader.GetString(2), // Name
                        reader.GetString(3), // Surname
                        DateTime.Parse(reader.GetString(4)) // Birthday
                    );
                    korisnici.Add(korisnik);
                }

                reader.Close();
                connection.Close();
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
                SqliteConnection connection = new SqliteConnection(_connectionString);
                connection.Open();

                string query = "SELECT Id, Username, Name, Surname, Birthday FROM Users WHERE Id = @id";
                SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                SqliteDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    Korisnik korisnik = new Korisnik(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                        DateTime.Parse(reader.GetString(4))
                    );

                    reader.Close();
                    connection.Close();
                    return korisnik;
                }

                reader.Close();
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Greška: " + e.Message);
            }

            return null;
        }

        public Korisnik Create(Korisnik korisnik)
        {
            SqliteConnection connection = new SqliteConnection(_connectionString);
            connection.Open();

            string query = @"INSERT INTO Users (Username, Name, Surname, Birthday)
                             VALUES (@Username, @Name, @Surname, @Birthday);
                             SELECT last_insert_rowid();";

            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@Username", korisnik.KorisnickoIme);
            command.Parameters.AddWithValue("@Name", korisnik.Ime);
            command.Parameters.AddWithValue("@Surname", korisnik.Prezime);
            command.Parameters.AddWithValue("@Birthday", korisnik.DatumRodjenja.ToString("yyyy-MM-dd"));

            long newId = (long)command.ExecuteScalar();
            connection.Close();

            return new Korisnik((int)newId, korisnik.KorisnickoIme, korisnik.Ime, korisnik.Prezime, korisnik.DatumRodjenja);
        }

        public bool Update(int id, Korisnik korisnik)
        {
            SqliteConnection connection = new SqliteConnection(_connectionString);
            connection.Open();

            string query = @"UPDATE Users SET 
                                Username = @Username, 
                                Name = @Name, 
                                Surname = @Surname, 
                                Birthday = @Birthday 
                             WHERE Id = @Id";

            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@Username", korisnik.KorisnickoIme);
            command.Parameters.AddWithValue("@Name", korisnik.Ime);
            command.Parameters.AddWithValue("@Surname", korisnik.Prezime);
            command.Parameters.AddWithValue("@Birthday", korisnik.DatumRodjenja.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@Id", id);

            int affectedRows = command.ExecuteNonQuery();
            connection.Close();

            return affectedRows > 0;
        }

        public bool Delete(int id)
        {
            SqliteConnection connection = new SqliteConnection(_connectionString);
            connection.Open();

            string query = "DELETE FROM Users WHERE Id = @Id";

            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);

            int affectedRows = command.ExecuteNonQuery();
            connection.Close();

            return affectedRows > 0;
        }
    }
}
