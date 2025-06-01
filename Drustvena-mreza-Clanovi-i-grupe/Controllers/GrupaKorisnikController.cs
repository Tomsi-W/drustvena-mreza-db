using Drustvena_mreza_Clanovi_i_grupe.Models;
using Drustvena_mreza_Clanovi_i_grupe.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Drustvena_mreza_Clanovi_i_grupe.Controllers
{
    [Route("api/grupa-korisnik/{grupaId}/korisnik")]
    [ApiController]
    public class GrupaKorisnikController : ControllerBase
    {
        private readonly KorisnikRepository _korisnikRepository;
        private readonly GrupaRepository _grupaRepository;

        // Dependency Injection constructor
        public GrupaKorisnikController(KorisnikRepository korisnikRepository, GrupaRepository grupaRepository)
        {
            _korisnikRepository = korisnikRepository;
            _grupaRepository = grupaRepository;
        }

        [HttpGet]
        public ActionResult<List<Korisnik>> GetClanovi(int grupaId)
        {
            try
            {
                Grupa? grupa = _grupaRepository.GetById(grupaId); // <<< NULL LEHET!

                if (grupa == null)
                {
                    return NotFound("Grupa ne postoji.");
                }

                return Ok(grupa.Korisnici);
            }
            catch (Exception)
            {
                return StatusCode(500, "Greška prilikom dohvatanja članova grupe.");
            }
        }
    }
}
