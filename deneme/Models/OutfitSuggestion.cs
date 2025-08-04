using System;

namespace deneme.Models
{
    public class OutfitSuggestion
    {
        public string Style { get; set; } = string.Empty;
        public string UstGiyim { get; set; } = string.Empty;
        public string AltGiyim { get; set; } = string.Empty;
        public string Ayakkabi { get; set; } = string.Empty;
        public string Aksesuar { get; set; } = string.Empty;
        public string ColorScheme { get; set; } = string.Empty;
        public string Occasion { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class OutfitPreferences
    {
        public string Occasion { get; set; } = string.Empty;
        public string BudgetRange { get; set; } = string.Empty;
        public string PreferredColors { get; set; } = string.Empty;
    }
} 