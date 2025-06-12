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
        private readonly GrupaDbRepository _grupaRepo;
        public GrupaController (GrupaDbRepository grupaRepo)
        {
            _grupaRepo = grupaRepo;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if(page < 1 || pageSize < 1)
            {
                return BadRequest("Page i PageSIze moraju biti veci od nule.");
            }
            try
            {
                List<Grupa> grupe = _grupaRepo.GetPaged(page, pageSize);
                int totalCount = _grupaRepo.CountAll();
                if(grupe == null)
                {
                    return NotFound("Ne postoji nijedna grupa.");
                }
                Object result = new
                {
                    Data = grupe,
                    TotalCount = totalCount
                };
                return Ok(result);
            }
            catch (Exception ex)
            {

                return Problem("Došlo je do greške prilikom dohvatanja grupa.");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            Grupa grupa = _grupaRepo.GetById(id);
            if(grupa == null)
            {
                return NotFound("Grupa ne postoji.");
            }
            return Ok(grupa);
        }
        //Kreiranje nove grupe
        [HttpPost]
        public IActionResult Create([FromBody] Grupa novaGrupa)
        {
            try
            {
                Grupa kreiranaGrupa = _grupaRepo.Create(novaGrupa);
                return Ok(kreiranaGrupa);
            }
            catch (Exception ex)
            {
                return Problem("Doslo je do greske prilikom snimanja podataka.");
            }
        }

        //Brisanje grupe po ID-ju
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                int obrisano = _grupaRepo.Delete(id);
                if(obrisano == 0)
                {
                    return NotFound("Grupa nije pornadjena.");
                }
                return Ok("Grupa je uspesno obrisana.");
            }
            catch (Exception ex)
            {
                return Problem("Doslo je do greske prilikom brisanja grupe.");
            }
        }

        //Azuriranje grupe
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Grupa azuriranaGrupa)
        {
            try
            {
                int izmenjeno = _grupaRepo.Update(id, azuriranaGrupa);
                if (izmenjeno == 0)
                    return NotFound("Grupa sa datim ID-jem ne postoji.");
                return Ok("Grupa je uspešno ažurirana.");
            }
            catch
            {
                return StatusCode(500, "Došlo je do greške pri ažuriranju grupe.");
            }
        }

        [HttpGet("{id}/korisnici")]
        public IActionResult GetGroupWithMembers(int id)
        {
            Grupa grupa = _grupaRepo.GetGroupWithMembers(id);
            if(grupa == null)
            {
                return NotFound("Grupa nije pronadjena.");
            }
            return Ok(grupa);
        }

    }
}