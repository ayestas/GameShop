namespace GameShop.Core.Entities
{
    public class Rent
    {
        public int Id { get; set; }
        public string DateRented { get; set; }
        public string Name { get; set; }
        public int VideogameId { get; set; }
    }
}
