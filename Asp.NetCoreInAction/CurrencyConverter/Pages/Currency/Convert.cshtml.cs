using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CurrencyConverter.Pages.Currency
{
    public class ConvertModel : PageModel
    {
        public void OnGet()
        {
        }
        
        [BindProperty]
        public InputModel Input { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            return RedirectToPage("Success");
        }

        public class InputModel
        {
            [Required]
            [StringLength(3, MinimumLength = 3)]
            [CurrencyCode("GBP", "USD", "CAD", "EUR")]
            public string CurrencyForm { get; set; }

            [Required]
            [StringLength(3, MinimumLength = 3)]
            [CurrencyCode("GBP", "CAD", "EUR")]
            public string CurrencyTo { get; set; }

            [Required]
            [Range(1, 1000)]
            public decimal Quantity { get; set; }
        }
    }
}
