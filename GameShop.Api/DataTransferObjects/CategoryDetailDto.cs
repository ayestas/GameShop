namespace GameShop.Api.DataTransferObjects
{
    public class CategoryDetailDto
    {
        public int Id { get; set; }
        public string Genre { get; set; }
        public ICollection<VideogameDetailDto> Videogames { get; set; }
    }
}
