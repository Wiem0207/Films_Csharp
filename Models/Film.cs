namespace WebApi.Models
{
    public class Film
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Poster { get; set; }
        public string imdb { get; set; }
        public int Year { get; set; }
        public Film(int Id, string Name, string Poster, string imdb, int Year)
        {
            Id = Id;
            Name = Name;
            Poster = Poster;
            imdb = imdb;
            Year = Year;
        }
    }
}