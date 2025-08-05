using System.ComponentModel.DataAnnotations;

namespace deneme.Models
{
    public class HomeViewModel
    {
        public List<Product> FlashSaleProducts { get; set; } = new();
        public List<Product> PopularProducts { get; set; } = new();
    }
}
