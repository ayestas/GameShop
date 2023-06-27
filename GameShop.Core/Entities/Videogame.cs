﻿namespace GameShop.Core.Entities
{
    public class Videogame
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PublishingDate { get; set; }
        public string Author { get; set; }
        public string GameMode { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}