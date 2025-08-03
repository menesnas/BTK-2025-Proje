using deneme.Data;
using deneme.Services;
using deneme.Models;
using Microsoft.AspNetCore.Mvc;

namespace deneme.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly GeminiService _gemini;

        public ProductController(ApplicationDbContext db, GeminiService gemini)
        {
            _db = db;
            _gemini = gemini;
        }

        [HttpGet("/Product/Details/{id:int}")]
        public IActionResult Details(int id)
        {
            var p = _db.Products.FirstOrDefault(x => x.Id == id);
            return p == null ? NotFound() : View(p);
        }

        //  Ajax çağrısı
        [HttpPost("/Product/AskGemini")]
        public async Task<IActionResult> AskGemini([FromBody] ChatDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Question))
                return BadRequest("Soru boş.");

            var product = _db.Products.FirstOrDefault(x => x.Id == dto.ProductId);
            if (product == null) return NotFound("Ürün yok.");

            var prompt =
                $"Ürün: {product.Name}  (Kategori: {product.Category}, Fiyat: {product.Price:c})\n" +
                $"Açıklama: {product.Description ?? "—"}   |   Puan: {product.Rating}/5\n\n" +
                $"Soru: {dto.Question}";

            var answer = await _gemini.AskAsync(prompt);
            return Ok(new { answer });
        }
    }

    public class ChatDto
    {
        public int ProductId { get; set; }
        public string Question { get; set; } = "";
    }
}