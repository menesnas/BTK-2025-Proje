using deneme.Data;
using deneme.Models;

namespace deneme.Data
{
    public static class SeedData
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // -- KATEGORİLER --
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { Name = "Kadın Giyim", Icon = "fas fa-tshirt", Color = "text-primary" },
                    new Category { Name = "Erkek Giyim", Icon = "fas fa-user-tie", Color = "text-info" },
                    new Category { Name = "Çocuk & Bebek", Icon = "fas fa-baby", Color = "text-warning" },
                    new Category { Name = "Ev & Yaşam", Icon = "fas fa-home", Color = "text-success" },
                    new Category { Name = "Elektronik", Icon = "fas fa-laptop", Color = "text-dark" },
                    new Category { Name = "Güzellik", Icon = "fas fa-gem", Color = "text-danger" }
                );
                context.SaveChanges();
            }

            // -- ÜRÜNLER --
            if (!context.Products.Any())
            {
                context.Products.AddRange(
                    new Product
                    {
                        Name = "Kadın Kışlık Mont",
                        Description = "Soğuk hava koşullarına karşı üstün koruma.",
                        Price = 199.99m,
                        OldPrice = 569.99m,
                        ImageUrl = "https://via.placeholder.com/300x300?text=Mont",
                        Category = "Kadın Giyim",
                        Rating = 4,
                        ReviewCount = 124,
                        DiscountPercentage = 65,
                        IsFlashSale = true,
                        IsPopular = true,
                        CreatedDate = DateTime.Now
                    },
                    new Product
                    {
                        Name = "Erkek Spor Ayakkabı",
                        Description = "Rahat tabanlı günlük spor ayakkabı.",
                        Price = 299.99m,
                        OldPrice = 599.99m,
                        ImageUrl = "https://via.placeholder.com/300x300?text=Ayakkabı",
                        Category = "Erkek Giyim",
                        Rating = 5,
                        ReviewCount = 89,
                        DiscountPercentage = 50,
                        IsFlashSale = true,
                        IsPopular = true,
                        CreatedDate = DateTime.Now
                    },
                    new Product
                    {
                        Name = "Bluetooth Kulaklık",
                        Description = "Kablosuz, uzun pil ömürlü kulaklık.",
                        Price = 149.99m,
                        OldPrice = 249.99m,
                        ImageUrl = "https://via.placeholder.com/300x300?text=Kulaklık",
                        Category = "Elektronik",
                        Rating = 4,
                        ReviewCount = 67,
                        DiscountPercentage = 40,
                        IsFlashSale = true,
                        IsPopular = false,
                        CreatedDate = DateTime.Now
                    },
                    new Product
                    {
                        Name = "Akıllı Saat",
                        Description = "Çok-fonksiyonlu, su geçirmez akıllı saat.",
                        Price = 1299.99m,
                        OldPrice = 1999.99m,
                        ImageUrl = "https://via.placeholder.com/300x300?text=Saat",
                        Category = "Elektronik",
                        Rating = 5,
                        ReviewCount = 156,
                        DiscountPercentage = 35,
                        IsFlashSale = false,
                        IsPopular = true,
                        CreatedDate = DateTime.Now
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
