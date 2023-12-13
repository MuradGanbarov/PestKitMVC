using System.ComponentModel.DataAnnotations;

namespace PestKit.Models
{
    public class Product
    {
        public int Id { get; set; }
       
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public decimal Price { get; set; }
        
        public string Img { get; set; }
    }
}
