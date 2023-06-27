namespace GameShop.Core.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Genre { get; set; }
        public ICollection<Videogame> Videogames { get; set; } = new HashSet<Videogame>();
    }
}
