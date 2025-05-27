using Drustvena_mreza_Clanovi_i_grupe.Models;
using Drustvena_mreza_Clanovi_i_grupe.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;


namespace Drustvena_mreza_Clanovi_i_grupe.Controllers
{
    [ApiController]
    [Route("api/grupa")]
    public class GrupaController : ControllerBase
    {
        //Repozitorijumi za grupe i korisnike
        private readonly GrupaRepository grupaRepo = new GrupaRepository();
        private readonly KorisnikRepository korisnikRepo = new KorisnikRepository();

        //Dohvatanje svih grupa
        [HttpGet]
        public IActionResult GetAll()
        {
            var grupe = GetAllFromDataase();
            return Ok(grupe);
        }

        //Kreiranje nove grupe
        [HttpPost]
        public IActionResult Create([FromBody] Grupa novaGrupa)
        {
            //Generesi novi ID
            int newId = 1;
            if (grupaRepo.Data.Any())
            {
                newId = grupaRepo.Data.Keys.Max() + 1;
            }
            novaGrupa.Id = newId;

            if (grupaRepo.Data.ContainsKey(novaGrupa.Id))
                return BadRequest("Grupa sa datim ID-jem već postoji.");

            grupaRepo.Data[novaGrupa.Id] = novaGrupa;
            grupaRepo.Save();
            return Ok(novaGrupa);
        }

        //Brisanje grupe po ID-ju
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!grupaRepo.Data.ContainsKey(id))
                return NotFound("Grupa nije pronađena.");

            grupaRepo.Data.Remove(id);
            grupaRepo.Save();
            return Ok("Grupa je uspešno obrisana.");
        }

        //Dodavanje korisnika u grupu
        [HttpPut("{idGrupe}/korisnici/{idKorisnika}")]
        public IActionResult AddMember(int idGrupe, int idKorisnika)
        {
            if (!grupaRepo.Data.TryGetValue(idGrupe, out var grupa))
                return NotFound("Grupa nije pronađena.");

            if (!korisnikRepo.Data.TryGetValue(idKorisnika, out var korisnik))
                return NotFound("Korisnik nije pronađen.");

            if (grupa.Korisnici.Any(k => k.Id == idKorisnika))
                return BadRequest("Korisnik je već član grupe.");

            grupa.Korisnici.Add(korisnik);
            grupaRepo.Save();
            return Ok("Korisnik je uspešno dodat u grupu.");
        }

        //Uklanjanje korisnika iz grupe
        [HttpDelete("{idGrupe}/korisnici/{idKorisnika}")]
        public IActionResult RemoveMember(int idGrupe, int idKorisnika)
        {
            if (!grupaRepo.Data.TryGetValue(idGrupe, out var grupa))
                return NotFound("Grupa nije pronađena.");

            var korisnik = grupa.Korisnici.FirstOrDefault(k => k.Id == idKorisnika);
            if (korisnik == null)
                return BadRequest("Korisnik nije član ove grupe.");

            grupa.Korisnici.Remove(korisnik);
            grupaRepo.Save();
            return Ok("Korisnik je uspešno uklonjen iz grupe.");
        }
        //dobavljanje korisnika jedne grupe
        [HttpGet("{id}/korisnik")]
        public IActionResult GetClanoviGrupe(int id)
        {
            if (!grupaRepo.Data.ContainsKey(id))
                return NotFound("Grupa nije pronađena.");

            return Ok(grupaRepo.Data[id].Korisnici);
        }

        //Koriscenje baze podataka
        private List<Grupa> GetAllFromDataase()
        {
            List<Grupa> grupe = new List<Grupa>();
            string connectionString = "Data Source=DataBase/socialnetwork.db";

            try
            {
                using (SqliteConnection connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    SqliteCommand command = connection.CreateCommand();

                    command.CommandText = "SELECT Id, Name, CreationDate FROM Groups";

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Grupa grupa = new Grupa(
                                Convert.ToInt32(reader["Id"]),
                                reader["Name"].ToString(),
                                DateTime.Parse(reader["CreationDate"].ToString())
                                );

                            grupe.Add(grupa);
                        }
                    }
                }
            }
            catch (SqliteException e)
            {

                Console.WriteLine($"Greška pri radu sa bazom: {e.Message}");
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
    }
}