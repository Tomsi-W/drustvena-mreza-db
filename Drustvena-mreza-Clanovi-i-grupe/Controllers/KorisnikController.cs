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
    }
}
