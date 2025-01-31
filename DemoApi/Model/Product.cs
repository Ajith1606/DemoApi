using System.ComponentModel.DataAnnotations;

namespace DemoApi.Model
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public Decimal Price { get; set; }
        public DateOnly DateOnly { get; set; }
    }
}
