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

        //Dohvatanje svih grupa
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
        ////Kreiranje nove grupe
        //[HttpPost]
        //public IActionResult Create([FromBody] Grupa novaGrupa)
        //{
        //    //Generesi novi ID
        //    int newId = 1;
        //    if (grupaRepo.Data.Any())
        //    {
        //        newId = grupaRepo.Data.Keys.Max() + 1;
        //    }
        //    novaGrupa.Id = newId;

        //    if (grupaRepo.Data.ContainsKey(novaGrupa.Id))
        //        return BadRequest("Grupa sa datim ID-jem već postoji.");

        //    grupaRepo.Data[novaGrupa.Id] = novaGrupa;
        //    grupaRepo.Save();
        //    return Ok(novaGrupa);
        //}

        ////Brisanje grupe po ID-ju
        //[HttpDelete("{id}")]
        //public IActionResult Delete(int id)
        //{
        //    if (!grupaRepo.Data.ContainsKey(id))
        //        return NotFound("Grupa nije pronađena.");

        //    grupaRepo.Data.Remove(id);
        //    grupaRepo.Save();
        //    return Ok("Grupa je uspešno obrisana.");
        //}

        ////Dodavanje korisnika u grupu
        //[HttpPut("{idGrupe}/korisnici/{idKorisnika}")]
        //public IActionResult AddMember(int idGrupe, int idKorisnika)
        //{
        //    if (!grupaRepo.Data.TryGetValue(idGrupe, out var grupa))
        //        return NotFound("Grupa nije pronađena.");

        //    if (!korisnikRepo.Data.TryGetValue(idKorisnika, out var korisnik))
        //        return NotFound("Korisnik nije pronađen.");

        //    if (grupa.Korisnici.Any(k => k.Id == idKorisnika))
        //        return BadRequest("Korisnik je već član grupe.");

        //    grupa.Korisnici.Add(korisnik);
        //    grupaRepo.Save();
        //    return Ok("Korisnik je uspešno dodat u grupu.");
        //}

        ////Uklanjanje korisnika iz grupe
        //[HttpDelete("{idGrupe}/korisnici/{idKorisnika}")]
        //public IActionResult RemoveMember(int idGrupe, int idKorisnika)
        //{
        //    if (!grupaRepo.Data.TryGetValue(idGrupe, out var grupa))
        //        return NotFound("Grupa nije pronađena.");

        //    var korisnik = grupa.Korisnici.FirstOrDefault(k => k.Id == idKorisnika);
        //    if (korisnik == null)
        //        return BadRequest("Korisnik nije član ove grupe.");

        //    grupa.Korisnici.Remove(korisnik);
        //    grupaRepo.Save();
        //    return Ok("Korisnik je uspešno uklonjen iz grupe.");
        //}
        ////dobavljanje korisnika jedne grupe
        //[HttpGet("{id}/korisnik")]
        //public IActionResult GetClanoviGrupe(int id)
        //{
        //    if (!grupaRepo.Data.ContainsKey(id))
        //        return NotFound("Grupa nije pronađena.");

        //    return Ok(grupaRepo.Data[id].Korisnici);
        //}

       
    }
}