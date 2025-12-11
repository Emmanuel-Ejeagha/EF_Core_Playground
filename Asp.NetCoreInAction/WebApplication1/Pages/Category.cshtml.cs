using Company.ClassLibrary1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Namespace
{
    public class CategoryModel : PageModel
    {
        private readonly ToDoService _service;
        public List<ToDoListModel> Items { get; set; }

        public CategoryModel(ToDoService service)
        {
            _service = service;
        }

        public ActionResult OnGet(string category)
        {
            Items = _service.GetItemsForCategory(category);
            return Page();
        }
    }
}
