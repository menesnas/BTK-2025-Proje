using deneme.Data;
using deneme.Models;

namespace deneme.Data
{
    public static class SeedData
    {
        public static void Initialize(ApplicationDbContext context)
        {

            // -- ÜRÜNLER --
            if (!context.Products.Any())
            {
                context.Products.AddRange(
                    new Product
                    {
                        Name = "Kadın Kışlık Mont",
                        Colour = "Kırmızı",
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
<<<<<<< HEAD
                        Name = "Spor Ayakkabı",
=======
                        Name = "Erkek Spor Ayakkabı",
>>>>>>> 58b4ee77e0fe94b2fff59c5dac536358bd791fe5
                        Colour = "Beyaz",
                        Description = "Rahat tabanlı günlük spor ayakkabı.",
                        Price = 299.99m,
                        OldPrice = 599.99m,
                        ImageUrl = "https://via.placeholder.com/300x300?text=Ayakkabı",
                        Category = "Ayakkabı",
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
                        Colour = "Siyah",
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
                        Colour = "Siyah",
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
                    new Product { Name = "Kadın Kazak", Colour = "Kırmızı", Description = "Yumuşak dokulu, sıcacık kazak.", Price = 129.99m, OldPrice = 229.99m, ImageUrl = "https://via.placeholder.com/300x300?text=Kazak", Category = "Kadın Giyim", Rating = 4, ReviewCount = 45, DiscountPercentage = 44, IsFlashSale = true, IsPopular = false, CreatedDate = DateTime.Now },
                    new Product { Name = "Kadın Jean Pantolon", Colour = "Siyah", Description = "Yüksek bel, rahat kesim pantolon.", Price = 149.99m, OldPrice = 289.99m, ImageUrl = "https://via.placeholder.com/300x300?text=Jean", Category = "Kadın Giyim", Rating = 4, ReviewCount = 68, DiscountPercentage = 48, IsFlashSale = true, IsPopular = true, CreatedDate = DateTime.Now },
                    new Product { Name = "Kadın Topuklu Ayakkabı", Colour = "Kırmızı", Description = "Şık ve zarif topuklu ayakkabı.", Price = 199.99m, OldPrice = 399.99m, ImageUrl = "https://via.placeholder.com/300x300?text=Topuklu", Category = "Kadın Giyim", Rating = 5, ReviewCount = 87, DiscountPercentage = 50, IsFlashSale = true, IsPopular = true, CreatedDate = DateTime.Now },
                    new Product { Name = "Kadın Şapka", Colour = "Beyaz", Description = "Yazlık şık plaj şapkası.", Price = 59.99m, OldPrice = 109.99m, ImageUrl = "https://via.placeholder.com/300x300?text=Şapka", Category = "Kadın Giyim", Rating = 4, ReviewCount = 33, DiscountPercentage = 45, IsFlashSale = false, IsPopular = false, CreatedDate = DateTime.Now },
                    new Product { Name = "Kadın Kolye", Colour = "Kırmızı", Description = "Zarif taşlı kolye modeli.", Price = 89.99m, OldPrice = 179.99m, ImageUrl = "https://via.placeholder.com/300x300?text=Kolye", Category = "Kadın Giyim", Rating = 4, ReviewCount = 41, DiscountPercentage = 50, IsFlashSale = true, IsPopular = true, CreatedDate = DateTime.Now },

                    // Erkek Giyim
                    new Product { Name = "Erkek Tişört", Colour = "Beyaz", Description = "Günlük giyime uygun rahat tişört.", Price = 79.99m, OldPrice = 149.99m, ImageUrl = "https://via.placeholder.com/300x300?text=Tişört", Category = "Erkek Giyim", Rating = 4, ReviewCount = 50, DiscountPercentage = 47, IsFlashSale = true, IsPopular = false, CreatedDate = DateTime.Now },
                    new Product { Name = "Erkek Kot Pantolon", Colour = "Siyah", Description = "Dayanıklı ve şık kot pantolon.", Price = 179.99m, OldPrice = 299.99m, ImageUrl = "https://via.placeholder.com/300x300?text=Pantolon", Category = "Erkek Giyim", Rating = 5, ReviewCount = 90, DiscountPercentage = 40, IsFlashSale = true, IsPopular = true, CreatedDate = DateTime.Now },
                    new Product { Name = "Erkek Deri Ayakkabı", Colour = "Siyah", Description = "Klasik stil, kaliteli deri ayakkabı.", Price = 299.99m, OldPrice = 499.99m, ImageUrl = "https://via.placeholder.com/300x300?text=DeriAyakkabı", Category = "Erkek Giyim", Rating = 5, ReviewCount = 78, DiscountPercentage = 40, IsFlashSale = false, IsPopular = true, CreatedDate = DateTime.Now },
                    new Product { Name = "Erkek Şapka", Colour = "Siyah", Description = "Spor tarzda şık şapka.", Price = 49.99m, OldPrice = 99.99m, ImageUrl = "https://via.placeholder.com/300x300?text=Şapka", Category = "Erkek Giyim", Rating = 4, ReviewCount = 21, DiscountPercentage = 50, IsFlashSale = false, IsPopular = false, CreatedDate = DateTime.Now },
                    new Product { Name = "Erkek Saat", Colour = "Siyah", Description = "Metal kordonlu klasik kol saati.", Price = 599.99m, OldPrice = 899.99m, ImageUrl = "https://via.placeholder.com/300x300?text=Saat", Category = "Erkek Giyim", Rating = 5, ReviewCount = 110, DiscountPercentage = 33, IsFlashSale = true, IsPopular = true, CreatedDate = DateTime.Now },

                    // Çocuk & Bebek
                    new Product { Name = "Bebek Tulum", Colour = "Beyaz", Description = "Yumuşak pamuklu bebek tulumu.", Price = 89.99m, OldPrice = 159.99m, ImageUrl = "https://via.placeholder.com/300x300?text=Tulum", Category = "Çocuk & Bebek", Rating = 5, ReviewCount = 38, DiscountPercentage = 43, IsFlashSale = true, IsPopular = true, CreatedDate = DateTime.Now },
                    new Product { Name = "Çocuk Ayakkabı", Colour = "Beyaz", Description = "Kaymaz tabanlı çocuk ayakkabısı.", Price = 129.99m, OldPrice = 229.99m, ImageUrl = "https://via.placeholder.com/300x300?text=ÇocukAyakkabı", Category = "Çocuk & Bebek", Rating = 4, ReviewCount = 44, DiscountPercentage = 43, IsFlashSale = false, IsPopular = false, CreatedDate = DateTime.Now },
                    new Product { Name = "Çocuk Mont", Colour = "Beyaz", Description = "Su geçirmez, kapüşonlu mont.", Price = 199.99m, OldPrice = 349.99m, ImageUrl = "https://via.placeholder.com/300x300?text=ÇocukMont", Category = "Çocuk & Bebek", Rating = 4, ReviewCount = 56, DiscountPercentage = 42, IsFlashSale = true, IsPopular = true, CreatedDate = DateTime.Now },
                    new Product { Name = "Bebek Şapka", Colour = "Beyaz", Description = "Sevimli desenli bebek şapkası.", Price = 29.99m, OldPrice = 69.99m, ImageUrl = "https://via.placeholder.com/300x300?text=BebekŞapka", Category = "Çocuk & Bebek", Rating = 4, ReviewCount = 12, DiscountPercentage = 57, IsFlashSale = false, IsPopular = false, CreatedDate = DateTime.Now },
                    new Product { Name = "Çocuk Kol Saati", Colour = "Beyaz", Description = "Renkli dijital saat.", Price = 49.99m, OldPrice = 99.99m, ImageUrl = "https://via.placeholder.com/300x300?text=ÇocukSaat", Category = "Çocuk & Bebek", Rating = 5, ReviewCount = 25, DiscountPercentage = 50, IsFlashSale = true, IsPopular = false, CreatedDate = DateTime.Now },
                    
                    // 10 Tişört
                    new Product { Name = "Basic Tişört", Colour = "Beyaz", Description = "Pamuklu beyaz tişört", Price = 149m, OldPrice = 163.90m, ImageUrl = "https://via.placeholder.com/300x300?text=Basic+Tişört", Category = "Tişört", Rating = 4, ReviewCount = 20, DiscountPercentage = 10, IsFlashSale = false, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-5) },
                    new Product { Name = "Oversize Tişört", Colour = "Siyah", Description = "Modern oversize kesim", Price = 169m, OldPrice = 194.35m, ImageUrl = "https://via.placeholder.com/300x300?text=Oversize+Tişört", Category = "Tişört", Rating = 5, ReviewCount = 34, DiscountPercentage = 15, IsFlashSale = true, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-3) },
                    new Product { Name = "V Yaka Tişört", Colour = "Gri", Description = "Şık v yaka tasarımı", Price = 129m, OldPrice = 135.45m, ImageUrl = "https://via.placeholder.com/300x300?text=V+Yaka+Tişört", Category = "Tişört", Rating = 4, ReviewCount = 12, DiscountPercentage = 5, IsFlashSale = false, IsPopular = false, CreatedDate = DateTime.Now.AddDays(-8) },
                    new Product { Name = "Baskılı Tişört", Colour = "Lacivert", Description = "Genç baskılı tişört", Price = 159m, OldPrice = 171.72m, ImageUrl = "https://via.placeholder.com/300x300?text=Baskılı+Tişört", Category = "Tişört", Rating = 4, ReviewCount = 18, DiscountPercentage = 8, IsFlashSale = true, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-2) },
                    new Product { Name = "Slim Fit Tişört", Colour = "Yeşil", Description = "Vücuda oturan tasarım", Price = 179m, OldPrice = 196.90m, ImageUrl = "https://via.placeholder.com/300x300?text=Slim+Fit+Tişört", Category = "Tişört", Rating = 5, ReviewCount = 25, DiscountPercentage = 10, IsFlashSale = false, IsPopular = false, CreatedDate = DateTime.Now.AddDays(-6) },
                    new Product { Name = "Şardonlu Tişört", Colour = "Kırmızı", Description = "Soğuk hava için ideal", Price = 189m, OldPrice = 211.68m, ImageUrl = "https://via.placeholder.com/300x300?text=Şardonlu+Tişört", Category = "Tişört", Rating = 3, ReviewCount = 9, DiscountPercentage = 12, IsFlashSale = false, IsPopular = false, CreatedDate = DateTime.Now.AddDays(-10) },
                    new Product { Name = "Beyzbol Tişört", Colour = "Beyaz/Siyah", Description = "Raglan kollu beyzbol stili", Price = 199m, OldPrice = 212.93m, ImageUrl = "https://via.placeholder.com/300x300?text=Beyzbol+Tişört", Category = "Tişört", Rating = 4, ReviewCount = 17, DiscountPercentage = 7, IsFlashSale = true, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-1) },
                    new Product { Name = "Crop Tişört", Colour = "Pembe", Description = "Kısa kesim crop modeli", Price = 139m, OldPrice = 152.90m, ImageUrl = "https://via.placeholder.com/300x300?text=Crop+Tişört", Category = "Tişört", Rating = 5, ReviewCount = 14, DiscountPercentage = 10, IsFlashSale = false, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-4) },
                    new Product { Name = "Yazlık Tişört", Colour = "Turuncu", Description = "Yaz ayları için nefes alır", Price = 129m, OldPrice = 135.45m, ImageUrl = "https://via.placeholder.com/300x300?text=Yazlık+Tişört", Category = "Tişört", Rating = 3, ReviewCount = 7, DiscountPercentage = 5, IsFlashSale = false, IsPopular = false, CreatedDate = DateTime.Now.AddDays(-9) },
                    new Product { Name = "Kolej Tişört", Colour = "Mavi", Description = "Kolej tarzı genç tasarım", Price = 169m, OldPrice = 182.52m, ImageUrl = "https://via.placeholder.com/300x300?text=Kolej+Tişört", Category = "Tişört", Rating = 4, ReviewCount = 11, DiscountPercentage = 8, IsFlashSale = false, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-7) },
                    
                    // 5 Etek
                    new Product { Name = "Deri Etek", Colour = "Siyah", Description = "Mini deri etek", Price = 229m, OldPrice = 256.48m, ImageUrl = "https://via.placeholder.com/300x300?text=Deri+Etek", Category = "Etek", Rating = 4, ReviewCount = 10, DiscountPercentage = 12, IsFlashSale = true, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-3) },
                    new Product { Name = "Pileli Etek", Colour = "Lila", Description = "Pile detaylı midi etek", Price = 199m, OldPrice = 214.92m, ImageUrl = "https://via.placeholder.com/300x300?text=Pileli+Etek", Category = "Etek", Rating = 4, ReviewCount = 9, DiscountPercentage = 8, IsFlashSale = false, IsPopular = false, CreatedDate = DateTime.Now.AddDays(-7) },
                    new Product { Name = "Kot Etek", Colour = "Açık Mavi", Description = "Günlük kot mini etek", Price = 189m, OldPrice = 207.90m, ImageUrl = "https://via.placeholder.com/300x300?text=Kot+Etek", Category = "Etek", Rating = 4, ReviewCount = 12, DiscountPercentage = 10, IsFlashSale = false, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-6) },
                    new Product { Name = "Saten Etek", Colour = "Yeşil", Description = "Parlak saten kumaş", Price = 219m, OldPrice = 251.85m, ImageUrl = "https://via.placeholder.com/300x300?text=Saten+Etek", Category = "Etek", Rating = 5, ReviewCount = 7, DiscountPercentage = 15, IsFlashSale = false, IsPopular = false, CreatedDate = DateTime.Now.AddDays(-5) },
                    new Product { Name = "Uzun Etek", Colour = "Krem", Description = "Rahat ve dökümlü kesim", Price = 249m, OldPrice = 266.43m, ImageUrl = "https://via.placeholder.com/300x300?text=Uzun+Etek", Category = "Etek", Rating = 3, ReviewCount = 5, DiscountPercentage = 7, IsFlashSale = true, IsPopular = false, CreatedDate = DateTime.Now.AddDays(-4) },

                    // 5 Pantolon
                    new Product { Name = "Kot Pantolon", Colour = "Mavi", Description = "Dar kesim kot pantolon", Price = 299m, OldPrice = 328.90m, ImageUrl = "https://via.placeholder.com/300x300?text=Kot+Pantolon", Category = "Pantolon", Rating = 4, ReviewCount = 22, DiscountPercentage = 10, IsFlashSale = false, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-3) },
                    new Product { Name = "Kargo Pantolon", Colour = "Haki", Description = "Yan cepli modern kargo pantolon", Price = 319m, OldPrice = 366.85m, ImageUrl = "https://via.placeholder.com/300x300?text=Kargo+Pantolon", Category = "Pantolon", Rating = 4, ReviewCount = 15, DiscountPercentage = 15, IsFlashSale = true, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-1) },
                    new Product { Name = "Şalvar Pantolon", Colour = "Siyah", Description = "Rahat kesim, etnik tarz", Price = 279m, OldPrice = 312.48m, ImageUrl = "https://via.placeholder.com/300x300?text=Şalvar+Pantolon", Category = "Pantolon", Rating = 3, ReviewCount = 8, DiscountPercentage = 12, IsFlashSale = false, IsPopular = false, CreatedDate = DateTime.Now.AddDays(-6) },
                    new Product { Name = "Kanvas Pantolon", Colour = "Bej", Description = "Günlük kullanıma uygun", Price = 299m, OldPrice = 319.93m, ImageUrl = "https://via.placeholder.com/300x300?text=Kanvas+Pantolon", Category = "Pantolon", Rating = 5, ReviewCount = 10, DiscountPercentage = 7, IsFlashSale = false, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-4) },
                    new Product { Name = "Jogger Pantolon", Colour = "Gri", Description = "Spor ve günlük kullanım", Price = 249m, OldPrice = 273.90m, ImageUrl = "https://via.placeholder.com/300x300?text=Jogger+Pantolon", Category = "Pantolon", Rating = 4, ReviewCount = 13, DiscountPercentage = 10, IsFlashSale = true, IsPopular = false, CreatedDate = DateTime.Now.AddDays(-2) },
                    // 5 Bluz
                    new Product { Name = "Şifon Bluz", Colour = "Beyaz", Description = "Hafif ve zarif", Price = 159m, OldPrice = 174.90m, ImageUrl = "https://via.placeholder.com/300x300?text=Şifon+Bluz", Category = "Bluz", Rating = 4, ReviewCount = 11, DiscountPercentage = 10, IsFlashSale = false, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-3) },
                    new Product { Name = "Dantel Bluz", Colour = "Siyah", Description = "Romantik ve şık", Price = 179m, OldPrice = 200.48m, ImageUrl = "https://via.placeholder.com/300x300?text=Dantel+Bluz", Category = "Bluz", Rating = 5, ReviewCount = 8, DiscountPercentage = 12, IsFlashSale = false, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-6) },
                    new Product { Name = "Omuz Açık Bluz", Colour = "Pembe", Description = "Yazlık ve şık tasarım", Price = 149m, OldPrice = 159.43m, ImageUrl = "https://via.placeholder.com/300x300?text=Omuz+Acik+Bluz", Category = "Bluz", Rating = 3, ReviewCount = 6, DiscountPercentage = 7, IsFlashSale = true, IsPopular = false, CreatedDate = DateTime.Now.AddDays(-2) },
                    new Product { Name = "Saten Bluz", Colour = "Mor", Description = "Gece kullanımı için ideal", Price = 199m, OldPrice = 218.90m, ImageUrl = "https://via.placeholder.com/300x300?text=Saten+Bluz", Category = "Bluz", Rating = 4, ReviewCount = 10, DiscountPercentage = 10, IsFlashSale = false, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-5) },
                    new Product { Name = "Basic Bluz", Colour = "Kırmızı", Description = "Günlük kullanım", Price = 139m, OldPrice = 145.95m, ImageUrl = "https://via.placeholder.com/300x300?text=Basic+Bluz", Category = "Bluz", Rating = 4, ReviewCount = 7, DiscountPercentage = 5, IsFlashSale = false, IsPopular = false, CreatedDate = DateTime.Now.AddDays(-1) },
                    // 10 Ayakkabı
                    new Product { Name = "Spor Ayakkabı", Colour = "Beyaz", Description = "Günlük spor ayakkabı", Price = 449, OldPrice = 0, ImageUrl = "", Category = "Ayakkabı", Rating = 5, ReviewCount = 44, DiscountPercentage = 10, IsFlashSale = true, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-3) },
                    new Product { Name = "Koşu Ayakkabısı", Colour = "Siyah", Description = "Rahat ve hafif yapı", Price = 499, OldPrice = 0, ImageUrl = "", Category = "Ayakkabı", Rating = 4, ReviewCount = 25, DiscountPercentage = 12, IsFlashSale = true, IsPopular = false, CreatedDate = DateTime.Now.AddDays(-2) },
                    new Product { Name = "Sneaker", Colour = "Krem", Description = "Trendy sneaker modeli", Price = 479, OldPrice = 0, ImageUrl = "", Category = "Ayakkabı", Rating = 4, ReviewCount = 30, DiscountPercentage = 15, IsFlashSale = false, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-4) },
                    new Product { Name = "Topuklu Ayakkabı", Colour = "Kırmızı", Description = "Zarif gece ayakkabısı", Price = 299, OldPrice = 0, ImageUrl = "", Category = "Ayakkabı", Rating = 3, ReviewCount = 10, DiscountPercentage = 8, IsFlashSale = false, IsPopular = false, CreatedDate = DateTime.Now.AddDays(-8) },
                    new Product { Name = "Loafer", Colour = "Lacivert", Description = "Rahat klasik tasarım", Price = 399, OldPrice = 0, ImageUrl = "", Category = "Ayakkabı", Rating = 4, ReviewCount = 20, DiscountPercentage = 10, IsFlashSale = false, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-7) },
                    new Product { Name = "Bot", Colour = "Kahverengi", Description = "Kışlık bot", Price = 599, OldPrice = 0, ImageUrl = "", Category = "Ayakkabı", Rating = 5, ReviewCount = 28, DiscountPercentage = 13, IsFlashSale = true, IsPopular = false, CreatedDate = DateTime.Now.AddDays(-6) },
                    new Product { Name = "Sandalet", Colour = "Bej", Description = "Yazlık sandalet", Price = 219, OldPrice = 0, ImageUrl = "", Category = "Ayakkabı", Rating = 3, ReviewCount = 5, DiscountPercentage = 7, IsFlashSale = false, IsPopular = false, CreatedDate = DateTime.Now.AddDays(-5) },
                    new Product { Name = "Outdoor Ayakkabı", Colour = "Gri", Description = "Dağ yürüyüşleri için uygun", Price = 699, OldPrice = 0, ImageUrl = "", Category = "Ayakkabı", Rating = 5, ReviewCount = 18, DiscountPercentage = 10, IsFlashSale = true, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-3) },
                    new Product { Name = "Terlik", Colour = "Sarı", Description = "Ev ve plaj kullanımı", Price = 149, OldPrice = 0, ImageUrl = "", Category = "Ayakkabı", Rating = 4, ReviewCount = 12, DiscountPercentage = 5, IsFlashSale = false, IsPopular = false, CreatedDate = DateTime.Now.AddDays(-1) },
                    new Product { Name = "Babet", Colour = "Pembe", Description = "Günlük şık babet", Price = 269, OldPrice = 0, ImageUrl = "", Category = "Ayakkabı", Rating = 4, ReviewCount = 14, DiscountPercentage = 9, IsFlashSale = false, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-2) },
                    // 10 Laptop
                    new Product { Name = "Laptop Pro X1", Colour = "Gümüş", Description = "16GB RAM, 1TB SSD, Intel i7", Price = 32500, OldPrice = 36111, ImageUrl = "", Category = "Laptop", Rating = 5, ReviewCount = 45, DiscountPercentage = 10, IsFlashSale = false, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-5) },
                    new Product { Name = "Gaming Laptop Y9", Colour = "Siyah", Description = "RTX 4060, 32GB RAM", Price = 38999, OldPrice = 44317, ImageUrl = "", Category = "Laptop", Rating = 5, ReviewCount = 50, DiscountPercentage = 12, IsFlashSale = true, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-3) },
                    new Product { Name = "Ultrabook Slim", Colour = "Beyaz", Description = "İnce tasarım, uzun pil ömrü", Price = 28999, OldPrice = 31521, ImageUrl = "", Category = "Laptop", Rating = 4, ReviewCount = 30, DiscountPercentage = 8, IsFlashSale = false, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-6) },
                    new Product { Name = "Notebook Eco", Colour = "Gri", Description = "Ekonomik ve hafif", Price = 19999, OldPrice = 22221, ImageUrl = "", Category = "Laptop", Rating = 4, ReviewCount = 25, DiscountPercentage = 10, IsFlashSale = false, IsPopular = false, CreatedDate = DateTime.Now.AddDays(-7) },
                    new Product { Name = "Laptop X5 Max", Colour = "Siyah", Description = "Intel i9, 32GB RAM, 2TB SSD", Price = 34999, OldPrice = 41175, ImageUrl = "", Category = "Laptop", Rating = 5, ReviewCount = 40, DiscountPercentage = 15, IsFlashSale = true, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-4) },
                    new Product { Name = "Laptop Student Edition", Colour = "Mavi", Description = "Öğrenciler için uygun", Price = 16999, OldPrice = 17894, ImageUrl = "", Category = "Laptop", Rating = 4, ReviewCount = 18, DiscountPercentage = 5, IsFlashSale = false, IsPopular = false, CreatedDate = DateTime.Now.AddDays(-8) },
                    new Product { Name = "Laptop Business", Colour = "Gri", Description = "Ofis işleri için ideal", Price = 27999, OldPrice = 30106, ImageUrl = "", Category = "Laptop", Rating = 4, ReviewCount = 22, DiscountPercentage = 7, IsFlashSale = false, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-9) },
                    new Product { Name = "Laptop Touchscreen", Colour = "Siyah", Description = "Dokunmatik ekranlı laptop", Price = 29999, OldPrice = 33332, ImageUrl = "", Category = "Laptop", Rating = 4, ReviewCount = 27, DiscountPercentage = 10, IsFlashSale = false, IsPopular = false, CreatedDate = DateTime.Now.AddDays(-2) },
                    new Product { Name = "Laptop Mini", Colour = "Beyaz", Description = "Kompakt ve hafif", Price = 14999, OldPrice = 16303, ImageUrl = "", Category = "Laptop", Rating = 3, ReviewCount = 10, DiscountPercentage = 8, IsFlashSale = true, IsPopular = false, CreatedDate = DateTime.Now.AddDays(-1) },
                    new Product { Name = "Laptop Creator Edition", Colour = "Gümüş", Description = "Yaratıcı profesyoneller için", Price = 41999, OldPrice = 47726, ImageUrl = "", Category = "Laptop", Rating = 5, ReviewCount = 35, DiscountPercentage = 12, IsFlashSale = true, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-3) },

                    // 10 Bilgisayar
                    new Product { Name = "Masaüstü Bilgisayar A1", Colour = "Siyah", Description = "Intel i5, 16GB RAM", Price = 15999m, OldPrice = 17598.90m, ImageUrl = "https://via.placeholder.com/300x300?text=Masaüstü+Bilgisayar+A1", Category = "Bilgisayar", Rating = 4, ReviewCount = 21, DiscountPercentage = 10, IsFlashSale = false, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-5) },
                    new Product { Name = "Gaming PC G200", Colour = "RGB", Description = "RTX 3060, 16GB RAM", Price = 24999m, OldPrice = 28748.85m, ImageUrl = "https://via.placeholder.com/300x300?text=Gaming+PC+G200", Category = "Bilgisayar", Rating = 5, ReviewCount = 33, DiscountPercentage = 15, IsFlashSale = true, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-3) },
                    new Product { Name = "Ofis Bilgisayarı", Colour = "Gri", Description = "Ofis işleri için yeterli performans", Price = 9999m, OldPrice = 10798.92m, ImageUrl = "https://via.placeholder.com/300x300?text=Ofis+Bilgisayarı", Category = "Bilgisayar", Rating = 3, ReviewCount = 12, DiscountPercentage = 8, IsFlashSale = false, IsPopular = false, CreatedDate = DateTime.Now.AddDays(-6) },
                    new Product { Name = "All-in-One Bilgisayar", Colour = "Beyaz", Description = "Ekran ve kasa bir arada", Price = 18999m, OldPrice = 20898.90m, ImageUrl = "https://via.placeholder.com/300x300?text=All-in-One+Bilgisayar", Category = "Bilgisayar", Rating = 4, ReviewCount = 17, DiscountPercentage = 10, IsFlashSale = false, IsPopular = false, CreatedDate = DateTime.Now.AddDays(-7) },
                    new Product { Name = "Mini PC", Colour = "Siyah", Description = "Kompakt mini bilgisayar", Price = 8999m, OldPrice = 9448.95m, ImageUrl = "https://via.placeholder.com/300x300?text=Mini+PC", Category = "Bilgisayar", Rating = 3, ReviewCount = 9, DiscountPercentage = 5, IsFlashSale = false, IsPopular = false, CreatedDate = DateTime.Now.AddDays(-8) },
                    new Product { Name = "Yüksek Performanslı PC", Colour = "Siyah", Description = "Video düzenleme ve render için ideal", Price = 29999m, OldPrice = 33598.88m, ImageUrl = "https://via.placeholder.com/300x300?text=Yüksek+Performanslı+PC", Category = "Bilgisayar", Rating = 5, ReviewCount = 28, DiscountPercentage = 12, IsFlashSale = true, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-2) },
                    new Product { Name = "Bilgisayar Öğrenci Seti", Colour = "Gri", Description = "Evde eğitim için uygun", Price = 10999m, OldPrice = 11878.92m, ImageUrl = "https://via.placeholder.com/300x300?text=Bilgisayar+Öğrenci+Seti", Category = "Bilgisayar", Rating = 4, ReviewCount = 15, DiscountPercentage = 8, IsFlashSale = false, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-4) },
                    new Product { Name = "Bilgisayar Home Edition", Colour = "Beyaz", Description = "Ev kullanıcıları için uygun", Price = 12999m, OldPrice = 13908.93m, ImageUrl = "https://via.placeholder.com/300x300?text=Bilgisayar+Home+Edition", Category = "Bilgisayar", Rating = 4, ReviewCount = 20, DiscountPercentage = 7, IsFlashSale = false, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-6) },
                    new Product { Name = "Bilgisayar Pro Set", Colour = "Siyah", Description = "Profesyonel işler için", Price = 22999m, OldPrice = 25298.90m, ImageUrl = "https://via.placeholder.com/300x300?text=Bilgisayar+Pro+Set", Category = "Bilgisayar", Rating = 5, ReviewCount = 30, DiscountPercentage = 10, IsFlashSale = true, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-1) },
                    new Product { Name = "Kasa Bilgisayar", Colour = "Siyah", Description = "Güçlü kasa tipi PC", Price = 13999m, OldPrice = 14838.94m, ImageUrl = "https://via.placeholder.com/300x300?text=Kasa+Bilgisayar", Category = "Bilgisayar", Rating = 4, ReviewCount = 13, DiscountPercentage = 6, IsFlashSale = false, IsPopular = false, CreatedDate = DateTime.Now.AddDays(-3) },
                    
                    // 10 Ceket
                    new Product { Name = "Deri Ceket", Colour = "Siyah", Description = "Hakiki deri klasik ceket", Price = 699m, OldPrice = 768.90m, ImageUrl = "https://via.placeholder.com/300x300?text=Deri+Ceket", Category = "Ceket", Rating = 5, ReviewCount = 20, DiscountPercentage = 10, IsFlashSale = true, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-5) },
                    new Product { Name = "Kot Ceket", Colour = "Mavi", Description = "Klasik kot tasarımı", Price = 499m, OldPrice = 538.92m, ImageUrl = "https://via.placeholder.com/300x300?text=Kot+Ceket", Category = "Ceket", Rating = 4, ReviewCount = 14, DiscountPercentage = 8, IsFlashSale = false, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-6) },
                    new Product { Name = "Bomber Ceket", Colour = "Haki", Description = "Modern spor stil", Price = 549m, OldPrice = 614.88m, ImageUrl = "https://via.placeholder.com/300x300?text=Bomber+Ceket", Category = "Ceket", Rating = 4, ReviewCount = 18, DiscountPercentage = 12, IsFlashSale = false, IsPopular = false, CreatedDate = DateTime.Now.AddDays(-7) },
                    new Product { Name = "Yün Ceket", Colour = "Kahverengi", Description = "Soğuk havalar için ideal", Price = 799m, OldPrice = 870.91m, ImageUrl = "https://via.placeholder.com/300x300?text=Yün+Ceket", Category = "Ceket", Rating = 4, ReviewCount = 11, DiscountPercentage = 9, IsFlashSale = true, IsPopular = false, CreatedDate = DateTime.Now.AddDays(-4) },
                    new Product { Name = "Şişme Mont", Colour = "Lacivert", Description = "Kışlık şişme mont", Price = 649m, OldPrice = 713.90m, ImageUrl = "https://via.placeholder.com/300x300?text=Şişme+Mont", Category = "Ceket", Rating = 4, ReviewCount = 22, DiscountPercentage = 10, IsFlashSale = true, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-2) },
                    new Product { Name = "Kadife Ceket", Colour = "Bordo", Description = "Şık kadife tasarımı", Price = 599m, OldPrice = 634.94m, ImageUrl = "https://via.placeholder.com/300x300?text=Kadife+Ceket", Category = "Ceket", Rating = 3, ReviewCount = 8, DiscountPercentage = 6, IsFlashSale = false, IsPopular = false, CreatedDate = DateTime.Now.AddDays(-8) },
                    new Product { Name = "Kaban", Colour = "Gri", Description = "Soğuk havalara özel kaban", Price = 899m, OldPrice = 1033.85m, ImageUrl = "https://via.placeholder.com/300x300?text=Kaban", Category = "Ceket", Rating = 5, ReviewCount = 25, DiscountPercentage = 15, IsFlashSale = true, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-3) },
                    new Product { Name = "Trençkot", Colour = "Bej", Description = "Yağmurluk tarzı trençkot", Price = 749m, OldPrice = 801.43m, ImageUrl = "https://via.placeholder.com/300x300?text=Trençkot", Category = "Ceket", Rating = 4, ReviewCount = 13, DiscountPercentage = 7, IsFlashSale = false, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-1) },
                    new Product { Name = "Parka", Colour = "Yeşil", Description = "Kapitone içlikli parka", Price = 829m, OldPrice = 903.61m, ImageUrl = "https://via.placeholder.com/300x300?text=Parka", Category = "Ceket", Rating = 4, ReviewCount = 17, DiscountPercentage = 9, IsFlashSale = false, IsPopular = false, CreatedDate = DateTime.Now.AddDays(-6) },
                    new Product { Name = "Yağmurluk", Colour = "Sarı", Description = "Su geçirmez yağmurluk", Price = 399m, OldPrice = 418.95m, ImageUrl = "https://via.placeholder.com/300x300?text=Yağmurluk", Category = "Ceket", Rating = 4, ReviewCount = 10, DiscountPercentage = 5, IsFlashSale = false, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-4) },
                                        
                                        
                    // 10 Masa
                    new Product { Name = "Ahşap Çalışma Masası", Colour = "Ceviz", Description = "Geniş ve sağlam masa", Price = 1999m, OldPrice = 2198.90m, ImageUrl = "https://via.placeholder.com/300x300?text=Ahşap+Çalışma+Masası", Category = "Masa", Rating = 5, ReviewCount = 22, DiscountPercentage = 10, IsFlashSale = true, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-3) },
                    new Product { Name = "Laptop Masası", Colour = "Beyaz", Description = "Kompakt taşınabilir masa", Price = 599m, OldPrice = 646.92m, ImageUrl = "https://via.placeholder.com/300x300?text=Laptop+Masası", Category = "Masa", Rating = 4, ReviewCount = 12, DiscountPercentage = 8, IsFlashSale = false, IsPopular = false, CreatedDate = DateTime.Now.AddDays(-2) },
                    new Product { Name = "Yemek Masası", Colour = "Kahverengi", Description = "6 kişilik geniş yemek masası", Price = 2999m, OldPrice = 3358.88m, ImageUrl = "https://via.placeholder.com/300x300?text=Yemek+Masası", Category = "Masa", Rating = 5, ReviewCount = 30, DiscountPercentage = 12, IsFlashSale = false, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-5) },
                    new Product { Name = "Z Katlanır Masa", Colour = "Siyah", Description = "Katlanabilir fonksiyonel masa", Price = 799m, OldPrice = 855.93m, ImageUrl = "https://via.placeholder.com/300x300?text=Z+Katlanır+Masa", Category = "Masa", Rating = 3, ReviewCount = 9, DiscountPercentage = 7, IsFlashSale = false, IsPopular = false, CreatedDate = DateTime.Now.AddDays(-6) },
                    new Product { Name = "Ofis Masası", Colour = "Gri", Description = "Çekmeceli ofis masası", Price = 1499m, OldPrice = 1648.90m, ImageUrl = "https://via.placeholder.com/300x300?text=Ofis+Masası", Category = "Masa", Rating = 4, ReviewCount = 15, DiscountPercentage = 10, IsFlashSale = false, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-7) },
                    new Product { Name = "Yuvarlak Masa", Colour = "Doğal Ahşap", Description = "Dekoratif yuvarlak masa", Price = 1799m, OldPrice = 1942.92m, ImageUrl = "https://via.placeholder.com/300x300?text=Yuvarlak+Masa", Category = "Masa", Rating = 4, ReviewCount = 18, DiscountPercentage = 8, IsFlashSale = true, IsPopular = false, CreatedDate = DateTime.Now.AddDays(-4) },
                    new Product { Name = "Cam Masa", Colour = "Şeffaf", Description = "Modern cam yüzeyli masa", Price = 2499m, OldPrice = 2748.90m, ImageUrl = "https://via.placeholder.com/300x300?text=Cam+Masa", Category = "Masa", Rating = 5, ReviewCount = 14, DiscountPercentage = 10, IsFlashSale = false, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-2) },
                    new Product { Name = "Bilgisayar Masası", Colour = "Siyah", Description = "Klavye raylı masa", Price = 1299m, OldPrice = 1389.93m, ImageUrl = "https://via.placeholder.com/300x300?text=Bilgisayar+Masası", Category = "Masa", Rating = 4, ReviewCount = 11, DiscountPercentage = 7, IsFlashSale = false, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-3) },
                    new Product { Name = "Çocuk Çalışma Masası", Colour = "Mavi", Description = "Çocuklara özel masa", Price = 999m, OldPrice = 1048.95m, ImageUrl = "https://via.placeholder.com/300x300?text=Çocuk+Çalışma+Masası", Category = "Masa", Rating = 3, ReviewCount = 6, DiscountPercentage = 5, IsFlashSale = false, IsPopular = false, CreatedDate = DateTime.Now.AddDays(-5) },
                    new Product { Name = "Salon Sehpası", Colour = "Beyaz", Description = "Dekoratif sehpa", Price = 699m, OldPrice = 754.92m, ImageUrl = "https://via.placeholder.com/300x300?text=Salon+Sehpası", Category = "Masa", Rating = 4, ReviewCount = 13, DiscountPercentage = 8, IsFlashSale = false, IsPopular = true, CreatedDate = DateTime.Now.AddDays(-1) }
                           
                    );
                context.SaveChanges();
            }
        }
    }
}
