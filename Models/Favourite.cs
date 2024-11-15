namespace WebApi.Models
{

    public class Favourite
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FilmId { get; set; }
        public Favourite(int Id, int UserId, int FilmId)
        {
            Id = Id;
            UserId = UserId;
            FilmId = FilmId;
        }


    }
}