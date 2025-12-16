using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CurrencyConverter.Pages.Currency
{
    public class SuccessModel : PageModel
    {
        [TempData]
        public string? ResultMessage { get; set; }
        
        [TempData]
        public string? ConvertedAmount { get; set; }
        
        [TempData]
        public string? FromCurrencySymbol { get; set; }
        
        [TempData]
        public string? ToCurrencySymbol { get; set; }
        
        [TempData]
        public string? OriginalAmount { get; set; }
        
        // Computed properties for easier access
        public decimal ConvertedAmountDecimal 
        { 
            get => decimal.TryParse(ConvertedAmount, out var result) ? result : 0;
        }
        
        public decimal OriginalAmountDecimal
        {
            get => decimal.TryParse(OriginalAmount, out var result) ? result : 0;
        }
        
        public decimal ExchangeRate
        {
            get
            {
                if (OriginalAmountDecimal > 0 && ConvertedAmountDecimal > 0)
                    return ConvertedAmountDecimal / OriginalAmountDecimal;
                return 0;
            }
        }
        
        public void OnGet()
        {
        }
        
        public IActionResult OnPostConvertAgain()
        {
            return RedirectToPage("Convert");
        }
    }
}