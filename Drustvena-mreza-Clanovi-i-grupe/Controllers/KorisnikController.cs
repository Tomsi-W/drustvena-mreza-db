using Drustvena_mreza_Clanovi_i_grupe.Models;
using Drustvena_mreza_Clanovi_i_grupe.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Drustvena_mreza_Clanovi_i_grupe.Controllers
{
    [Route("api/korisnik")]
    [ApiController]
    public class KorisnikController : ControllerBase
    {
        private readonly KorisnikRepository _korisnikRepository;

        public KorisnikController(KorisnikRepository korisnikRepository)
        {
            _korisnikRepository = korisnikRepository;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                if (page < 1 || pageSize < 1)
                {
                    return BadRequest("Page i pageSize moraju biti veći od nule.");
                }

                List<Korisnik> korisnici = _korisnikRepository.GetAll(page, pageSize);
                int totalCount = _korisnikRepository.CountAll();

                if (korisnici == null || korisnici.Count == 0)
                {
                    return NotFound("Nema korisnika.");
                }

                var result = new
                {
                    data = korisnici,
                    totalCount = totalCount
                };

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Greška prilikom dohvatanja korisnika.");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                Korisnik? korisnik = _korisnikRepository.GetById(id);
                if (korisnik == null)
                {
                    return NotFound($"Korisnik sa ID-jem {id} nije pronađen.");
                }

                return Ok(korisnik);
            }
            catch (Exception)
            {
                return StatusCode(500, "Greška prilikom dohvatanja korisnika.");
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] Korisnik noviKorisnik)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(noviKorisnik.KorisnickoIme) || string.IsNullOrWhiteSpace(noviKorisnik.Ime))
                {
                    return BadRequest("Obavezna polja nisu popunjena.");
                }

                Korisnik kreiraniKorisnik = _korisnikRepository.Create(noviKorisnik);
                return CreatedAtAction(nameof(GetById), new { id = kreiraniKorisnik.Id }, kreiraniKorisnik);
            }
            catch (Exception)
            {
                return StatusCode(500, "Greška prilikom kreiranja korisnika.");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Korisnik korisnikZaIzmenu)
        {
            try
            {
                bool uspesno = _korisnikRepository.Update(id, korisnikZaIzmenu);
                if (!uspesno)
                {
                    return NotFound($"Korisnik sa ID-jem {id} ne postoji.");
                }

                return Ok("Korisnik je uspešno ažuriran.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Greška prilikom ažuriranja korisnika.");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                bool uspesno = _korisnikRepository.Delete(id);
                if (!uspesno)
                {
                    return NotFound($"Korisnik sa ID-jem {id} ne postoji.");
                }

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Greška prilikom brisanja korisnika.");
            }
        }
    }
}
