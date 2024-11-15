using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavouriteController : ControllerBase
    {
        // Nouveau Commentaire Ahmed
        // GET: api/<FavouriteController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<FavouriteController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<FavouriteController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<FavouriteController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<FavouriteController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
