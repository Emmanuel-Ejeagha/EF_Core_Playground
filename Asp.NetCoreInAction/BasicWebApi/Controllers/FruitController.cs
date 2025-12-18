using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BasicWebApi.Controllers
{
    // [Route("api/[controller]")]
    [ApiController]
    public class FruitController : ControllerBase
    {
        List<string> _fruit = new List<string>
        {
            "Pear",
            "Guava",
            "Apple",
            "Pineapple",
            "Pawpaw"
        };

        [HttpGet("fruit")]
        public IEnumerable<string> Index()
        {
            return _fruit;
        }

        [HttpGet("fruit/{id}")]
        public ActionResult<string> GetFruit(int id)
        {
            if (id >= 0 && id < _fruit.Count)
            {
                return _fruit[id];
            }
            
            return NotFound();
        }
    }
}
