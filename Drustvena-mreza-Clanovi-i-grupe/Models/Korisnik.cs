namespace Drustvena_mreza_Clanovi_i_grupe.Models
{
    public class Korisnik
    {
        private int v1;
        private string? v2;
        private string? v3;

        public int Id { get; set; }
        public string KorisnickoIme { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public DateTime DatumRodjenja { get; set; }

        public Korisnik()
        {
        }

        public Korisnik(int id, string korisnickoIme, string ime, string prezime, DateTime datumRodjenja)
        {
            Id = id;
            KorisnickoIme = korisnickoIme;
            Ime = ime;
            Prezime = prezime;
            DatumRodjenja = datumRodjenja;
        }

        public Korisnik(int v1, string? v2, string? v3)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
        }
    }
}
