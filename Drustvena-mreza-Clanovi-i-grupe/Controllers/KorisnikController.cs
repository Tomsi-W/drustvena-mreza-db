using Drustvena_mreza_Clanovi_i_grupe.Models;
using Drustvena_mreza_Clanovi_i_grupe.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Drustvena_mreza_Clanovi_i_grupe.Controllers
{
    [Route("api/korisnik")]
    [ApiController]
    public class KorisnikController : ControllerBase
    {
        private readonly KorisnikDbRepository korisnikRepo = new KorisnikDbRepository();

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var korisnici = korisnikRepo.GetAll();
                return Ok(korisnici);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greška prilikom dohvatanja korisnika: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var korisnik = korisnikRepo.GetById(id);
                if (korisnik == null)
                    return NotFound($"Korisnik sa ID-jem {id} nije pronađen.");

                return Ok(korisnik);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greška: {ex.Message}");
            }
        }
        [HttpPost]
        public IActionResult Create([FromBody] Korisnik noviKorisnik)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(noviKorisnik.KorisnickoIme) || string.IsNullOrWhiteSpace(noviKorisnik.Ime))
                    return BadRequest("Obavezna polja nisu popunjena.");

                Korisnik kreiraniKorisnik = korisnikRepo.Create(noviKorisnik);
                return CreatedAtAction(nameof(GetById), new { id = kreiraniKorisnik.Id }, kreiraniKorisnik);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greška prilikom kreiranja korisnika: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Korisnik korisnikZaIzmenu)
        {
            try
            {
                bool uspesno = korisnikRepo.Update(id, korisnikZaIzmenu);
                if (!uspesno)
                    return NotFound($"Korisnik sa ID-jem {id} ne postoji.");

                return Ok("Korisnik je uspešno ažuriran.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greška prilikom ažuriranja: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                bool uspesno = korisnikRepo.Delete(id);
                if (!uspesno)
                    return NotFound($"Korisnik sa ID-jem {id} ne postoji.");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greška prilikom brisanja: {ex.Message}");
            }
        }

    }
}
