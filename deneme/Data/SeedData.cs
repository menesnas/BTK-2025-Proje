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
                    },
                    // Kadın Giyim
                    new Product { Name = "Kadın Kazak", Description = "Yumuşak dokulu, sıcacık kazak.", Price = 129.99m, OldPrice = 229.99m, ImageUrl = "https://via.placeholder.com/300x300?text=Kazak", Category = "Kadın Giyim", Rating = 4, ReviewCount = 45, DiscountPercentage = 44, IsFlashSale = true, IsPopular = false, CreatedDate = DateTime.Now },
                    new Product { Name = "Kadın Jean Pantolon", Description = "Yüksek bel, rahat kesim pantolon.", Price = 149.99m, OldPrice = 289.99m, ImageUrl = "https://via.placeholder.com/300x300?text=Jean", Category = "Kadın Giyim", Rating = 4, ReviewCount = 68, DiscountPercentage = 48, IsFlashSale = true, IsPopular = true, CreatedDate = DateTime.Now },
                    new Product { Name = "Kadın Topuklu Ayakkabı", Description = "Şık ve zarif topuklu ayakkabı.", Price = 199.99m, OldPrice = 399.99m, ImageUrl = "https://via.placeholder.com/300x300?text=Topuklu", Category = "Kadın Giyim", Rating = 5, ReviewCount = 87, DiscountPercentage = 50, IsFlashSale = true, IsPopular = true, CreatedDate = DateTime.Now },
                    new Product { Name = "Kadın Şapka", Description = "Yazlık şık plaj şapkası.", Price = 59.99m, OldPrice = 109.99m, ImageUrl = "https://via.placeholder.com/300x300?text=Şapka", Category = "Kadın Giyim", Rating = 4, ReviewCount = 33, DiscountPercentage = 45, IsFlashSale = false, IsPopular = false, CreatedDate = DateTime.Now },
                    new Product { Name = "Kadın Kolye", Description = "Zarif taşlı kolye modeli.", Price = 89.99m, OldPrice = 179.99m, ImageUrl = "https://via.placeholder.com/300x300?text=Kolye", Category = "Kadın Giyim", Rating = 4, ReviewCount = 41, DiscountPercentage = 50, IsFlashSale = true, IsPopular = true, CreatedDate = DateTime.Now },

                    // Erkek Giyim
                    new Product { Name = "Erkek Tişört", Description = "Günlük giyime uygun rahat tişört.", Price = 79.99m, OldPrice = 149.99m, ImageUrl = "https://via.placeholder.com/300x300?text=Tişört", Category = "Erkek Giyim", Rating = 4, ReviewCount = 50, DiscountPercentage = 47, IsFlashSale = true, IsPopular = false, CreatedDate = DateTime.Now },
                    new Product { Name = "Erkek Kot Pantolon", Description = "Dayanıklı ve şık kot pantolon.", Price = 179.99m, OldPrice = 299.99m, ImageUrl = "https://via.placeholder.com/300x300?text=Pantolon", Category = "Erkek Giyim", Rating = 5, ReviewCount = 90, DiscountPercentage = 40, IsFlashSale = true, IsPopular = true, CreatedDate = DateTime.Now },
                    new Product { Name = "Erkek Deri Ayakkabı", Description = "Klasik stil, kaliteli deri ayakkabı.", Price = 299.99m, OldPrice = 499.99m, ImageUrl = "https://via.placeholder.com/300x300?text=DeriAyakkabı", Category = "Erkek Giyim", Rating = 5, ReviewCount = 78, DiscountPercentage = 40, IsFlashSale = false, IsPopular = true, CreatedDate = DateTime.Now },
                    new Product { Name = "Erkek Şapka", Description = "Spor tarzda şık şapka.", Price = 49.99m, OldPrice = 99.99m, ImageUrl = "https://via.placeholder.com/300x300?text=Şapka", Category = "Erkek Giyim", Rating = 4, ReviewCount = 21, DiscountPercentage = 50, IsFlashSale = false, IsPopular = false, CreatedDate = DateTime.Now },
                    new Product { Name = "Erkek Saat", Description = "Metal kordonlu klasik kol saati.", Price = 599.99m, OldPrice = 899.99m, ImageUrl = "https://via.placeholder.com/300x300?text=Saat", Category = "Erkek Giyim", Rating = 5, ReviewCount = 110, DiscountPercentage = 33, IsFlashSale = true, IsPopular = true, CreatedDate = DateTime.Now },

                    // Çocuk & Bebek
                    new Product { Name = "Bebek Tulum", Description = "Yumuşak pamuklu bebek tulumu.", Price = 89.99m, OldPrice = 159.99m, ImageUrl = "https://via.placeholder.com/300x300?text=Tulum", Category = "Çocuk & Bebek", Rating = 5, ReviewCount = 38, DiscountPercentage = 43, IsFlashSale = true, IsPopular = true, CreatedDate = DateTime.Now },
                    new Product { Name = "Çocuk Ayakkabı", Description = "Kaymaz tabanlı çocuk ayakkabısı.", Price = 129.99m, OldPrice = 229.99m, ImageUrl = "https://via.placeholder.com/300x300?text=ÇocukAyakkabı", Category = "Çocuk & Bebek", Rating = 4, ReviewCount = 44, DiscountPercentage = 43, IsFlashSale = false, IsPopular = false, CreatedDate = DateTime.Now },
                    new Product { Name = "Çocuk Mont", Description = "Su geçirmez, kapüşonlu mont.", Price = 199.99m, OldPrice = 349.99m, ImageUrl = "https://via.placeholder.com/300x300?text=ÇocukMont", Category = "Çocuk & Bebek", Rating = 4, ReviewCount = 56, DiscountPercentage = 42, IsFlashSale = true, IsPopular = true, CreatedDate = DateTime.Now },
                    new Product { Name = "Bebek Şapka", Description = "Sevimli desenli bebek şapkası.", Price = 29.99m, OldPrice = 69.99m, ImageUrl = "https://via.placeholder.com/300x300?text=BebekŞapka", Category = "Çocuk & Bebek", Rating = 4, ReviewCount = 12, DiscountPercentage = 57, IsFlashSale = false, IsPopular = false, CreatedDate = DateTime.Now },
                    new Product { Name = "Çocuk Kol Saati", Description = "Renkli dijital saat.", Price = 49.99m, OldPrice = 99.99m, ImageUrl = "https://via.placeholder.com/300x300?text=ÇocukSaat", Category = "Çocuk & Bebek", Rating = 5, ReviewCount = 25, DiscountPercentage = 50, IsFlashSale = true, IsPopular = false, CreatedDate = DateTime.Now }
                );
                context.SaveChanges();
            }
        }
    }
}
