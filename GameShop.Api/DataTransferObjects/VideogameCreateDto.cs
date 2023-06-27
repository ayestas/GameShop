namespace GameShop.Api.DataTransferObjects
{
    public class VideogameCreateDto
    {
        public string Name { get; set; }
        public string PublishingDate { get; set; }
        public string Author { get; set; }
        public string GameMode { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
    }
}
