using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApiController
{
    [Route("api/[controller]")]
    [ApiController]
    public class FruitController : ControllerBase
    {
        private List<string> _fruit = new List<string>
        {
            "Pear",
            "Grape",
            "Lime",
            "Lemon"
        };

        [HttpGet("fruit")]
        public IEnumerable<string> GetAllFruit()
        {
            return _fruit;
        }

        [HttpGet("fruit/{id}")]
        public ActionResult<string> GetFruitById(int id)
        {
            if (id >= 0 || id < _fruit.Count)
            {
                return _fruit[id];
            }
            return NotFound();
        }

        [HttpPost("fruit")]
        public ActionResult CreatFruit(UpdateModel model)
        {
            if (model.Id < 0 || model.Id > _fruit.Count)
            {
                return NotFound();
            }
            _fruit[model.Id] = model.Name;

            return Ok();
        }

        private string[] _cars = new string[]
            {
                "BMW", "Mercedes Benz", "Ford", "Toyota", "Lexus"
            };

        [HttpGet("/api/car")]
        public IEnumerable<string> GetCars()
        {
            return _cars;
        }

        [HttpGet("/api/car/{id}")]
        public ActionResult<string> GetCarById(int id)
        {
            if (id < 0 || id >= _cars.Length)
            {
                return NotFound($"Page not found please enter the correct id");
            }
            return _cars[id];
        }

    }
    
    

    public class UpdateModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
