using System.ComponentModel.DataAnnotations;
using CurrencyConverter.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CurrencyConverter.Pages.Currency
{
    public class ConvertModel : PageModel
    {
        private readonly ICurrencyService _currencyService;
        
        public ConvertModel(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
            AvailableCurrencies = new Dictionary<string, string>();
            Input = new InputModel();
        }

        [BindProperty]
        public InputModel Input { get; set; }
        
        [TempData]
        public string? ResultMessage { get; set; }
        
        [TempData]
        public string? ConvertedAmount { get; set; } // Changed to string
        
        [TempData]
        public string? FromCurrencySymbol { get; set; }
        
        [TempData]
        public string? ToCurrencySymbol { get; set; }
        
        [TempData]
        public string? OriginalAmount { get; set; } // New: store original amount
        
        public Dictionary<string, string> AvailableCurrencies { get; set; }

        public void OnGet()
        {
            AvailableCurrencies = _currencyService.GetCurrencyNames();
        }
        
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                AvailableCurrencies = _currencyService.GetCurrencyNames();
                return Page();
            }
            
            // Perform conversion
            var converted = _currencyService.ConvertCurrency(
                Input.Quantity, 
                Input.CurrencyFrom, 
                Input.CurrencyTo);
            
            // Store results in TempData as strings
            OriginalAmount = Input.Quantity.ToString("N2");
            ConvertedAmount = converted.ToString("N2");
            FromCurrencySymbol = Input.CurrencyFrom;
            ToCurrencySymbol = Input.CurrencyTo;
            
            ResultMessage = $"{Input.Quantity:N2} {Input.CurrencyFrom} = {converted:N2} {Input.CurrencyTo}";
            
            return RedirectToPage("Success");
        }

        public class InputModel
        {
            [Required]
            [CurrencyCode("GBP", "USD", "CAD", "EUR", "JPY", "AUD")]
            [Display(Name = "From Currency")]
            public string CurrencyFrom { get; set; } = "USD";

            [Required]
            [CurrencyCode("GBP", "USD", "CAD", "EUR", "JPY", "AUD")]
            [Display(Name = "To Currency")]
            public string CurrencyTo { get; set; } = "EUR";

            [Required]
            [Range(0.01, 1000000)]
            [Display(Name = "Amount")]
            public decimal Quantity { get; set; } = 100.00m;
        }
    }
}