﻿using PestKit.Models;

namespace PestKit.ViewModels
{
    public class BasketItemVM
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public int? Count { get; set; }
        public decimal? SubTotal { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string? Img { get; set; }

    }
}
