namespace GameShop.Api.DataTransferObjects
{
    public class RentDetailDto
    {
        public int Id { get; set; }
        public string DateRented { get; set; }
        public string Name { get; set; }
        public int VideogameId { get; set; }
    }
}
