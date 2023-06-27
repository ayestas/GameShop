namespace GameShop.Api.DataTransferObjects
{
    public class RentCreateDto
    {
        public string DateRented { get; set; }
        public string Name { get; set; }
        public int VideogameId { get; set; }
    }
}
