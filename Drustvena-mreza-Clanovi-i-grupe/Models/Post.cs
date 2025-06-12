namespace Drustvena_mreza_Clanovi_i_grupe.Models
{
    public class Post
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string Content { get; set; }
        public string Date { get; set; }
        public string Username { get; set; } // ima korisnikovo
    }
}
