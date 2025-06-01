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
                    return BadRequest("Page and pageSize must be greater than 0.");
                }

                List<Korisnik> korisnici = _korisnikRepository.GetAll(page, pageSize);

                if (korisnici == null || korisnici.Count == 0)
                {
                    return NotFound("Nema korisnika.");
                }

                return Ok(korisnici);
            }
            catch (Exception)
            {
                return StatusCode(500, "Greška prilikom dohvatanja korisnika.");
            }
        }
    }
}
