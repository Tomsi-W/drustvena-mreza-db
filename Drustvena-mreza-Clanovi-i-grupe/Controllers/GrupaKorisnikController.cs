using Drustvena_mreza_Clanovi_i_grupe.Models;
using Drustvena_mreza_Clanovi_i_grupe.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Drustvena_mreza_Clanovi_i_grupe.Controllers
{
    [Route("api/grupa-korisnik/{grupaId}/korisnik")]
    [ApiController]
    public class GrupaKorisnikController : ControllerBase
    {
        private readonly KorisnikRepository _korisnikRepository;
        private readonly GrupaRepository _grupaRepository;
        private readonly GroupMembershipDbRepository _membershipRepository;

        // Dependency Injection constructor
        public GrupaKorisnikController(KorisnikRepository korisnikRepository, GrupaRepository grupaRepository, GroupMembershipDbRepository membershipRepository)
        {
            _korisnikRepository = korisnikRepository;
            _grupaRepository = grupaRepository;
            _membershipRepository = membershipRepository;
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
                return StatusCode(500, $"Greška prilikom dohvatanja članova grupe.");
            }
        }

        [HttpPut("{korisnikId}")]
        public IActionResult DodajKorisnikaUGrupu(int grupaId, int korisnikId)
        {
            try
            {
                int rezultat = _membershipRepository.AddUserToGroup(korisnikId, grupaId);
                if (rezultat == 0)
                    return BadRequest("Dodavanje korisnika u grupu nije uspelo.");

                return Ok("Korisnik je uspesno dodat u grupu.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greška: {ex.Message}"); 
            }
        }

        [HttpDelete("{korisnikId}")]
        public IActionResult UkloniKorisnikaIzGrupe(int grupaId, int korisnikId)
        {
            try
            {
                int rezultat = _membershipRepository.RemoveUserFromGroup(korisnikId, grupaId);
                if (rezultat == 0)
                    return NotFound("Korisnik nije pronađen u grupi.");

                return Ok("Korisnik je uspešno uklonjen iz grupe.");
            }
            catch
            {
                return StatusCode(500, "Greška prilikom uklanjanja korisnika iz grupe.");
            }
        }
    }
}
