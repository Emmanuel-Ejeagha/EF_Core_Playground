using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiControllerAttrb.Controllers
{
    // Creating a web API controller without the [ApiController] attribute
    public class FruitContollers : ControllerBase
    {
        List<string> _fruit = new List<string>
        {
            "Cucumber",
            "Mango",
            "Cherry",
            "Strawberry"
        };

        [HttpPost("fruit")]
        public ActionResult Update([FromBody] UpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    new ValidationProblemDetails(ModelState));
            }

            if (model.Id < 0 || model.Id >= _fruit.Count)
            {
                return NotFound(new ProblemDetails()
                {
                    Status = 404,
                    Title = "Not Found",
                    Type = "https://tools.ietf.org/html/rfc7231"
                        + "#section-6.5.4",
                });
            }
            _fruit[model.Id] = model.Name;
            return Ok();
        }

        [HttpGet("fruit")]
        public IEnumerable<string> GetFruit()
        {
            return _fruit;
        }
        
        [HttpGet("fruit/{id}")]
        public ActionResult<string> GetFruitbyId(int id)
        {
            if (id >= 0 && id < _fruit.Count)
            {
                return _fruit[id];
            }
            
            return NotFound();
        }
        
        public class UpdateModel
        {
            public int Id { get; set; }

            [Required]
            public string Name { get; set; }
        }
    }
}

