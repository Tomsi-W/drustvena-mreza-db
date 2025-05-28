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
        private readonly GrupaDbRepository grupaRepo = new GrupaDbRepository();

        [HttpGet]
        public IActionResult GetAll()
        {
            var grupe = grupaRepo.GetAll();
            return Ok(grupe);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            Grupa grupa = grupaRepo.GetById(id);
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
                Grupa kreiranaGrupa = grupaRepo.Create(novaGrupa);
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
                int obrisano = grupaRepo.Delete(id);
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
                int izmenjeno = grupaRepo.Update(id, azuriranaGrupa);
                if (izmenjeno == 0)
                    return NotFound("Grupa sa datim ID-jem ne postoji.");
                return Ok("Grupa je uspešno ažurirana.");
            }
            catch
            {
                return StatusCode(500, "Došlo je do greške pri ažuriranju grupe.");
            }
        }

    }
}