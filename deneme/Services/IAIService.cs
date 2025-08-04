using deneme.Models;

namespace deneme.Services
{
    public interface IAIService
    {
        Task<OutfitSuggestion> GetOutfitSuggestionAsync(string imageDescription, OutfitPreferences preferences, List<Product> productList);
    }
} 