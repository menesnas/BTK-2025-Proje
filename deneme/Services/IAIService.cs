using deneme.Models;
using Microsoft.AspNetCore.Http;

namespace deneme.Services
{
    public interface IAIService
    {
        Task<OutfitSuggestion> GetOutfitSuggestionAsync(string imageDescription, OutfitPreferences preferences, List<Product> productList);
        Task<(string description, string clothingType, string gender)> AnalyzeClothingImageAsync(IFormFile imageFile);
    }
} 