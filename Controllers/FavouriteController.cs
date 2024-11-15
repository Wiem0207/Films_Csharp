//using Microsoft.AspNetCore.Mvc;
//using WebApi.Models;
//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Identity;
//namespace WebApi.Models
//{
//    public class FavouriteInfo
//    {
//        public int Id { get; set; }
//        public int UserId { get; set; }
//        public int FilmId { get; set; }
//    }/////dans models
//}

//namespace WebApi.Controllers
//{
//    [ApiController]
//    [Route("api/favourite")]
//    [Route("api/favourite")]
//    public class FavouriteController : ControllerBase
//    {
//        private readonly BddContext _context;
//        private readonly PasswordHasher<User> _passwordHasher;  // Utilisation directe de PasswordHasher<User>

//        // Injection du contexte de données dans le constructeur
//        public FavouriteController(BddContext context)
//        {
//            _context = context;
//            _passwordHasher = new PasswordHasher<User>();  // Instanciation de PasswordHasher<User>
//        }
//        [HttpGet("{id}")]
//        public async Task<ActionResult<Favourite>> GetFavourite(int id)
//        {
//            // on récupère la confiture correspondant a l'id
//            var Favourite = await _context.Favourites.FindAsync(id);

//            if (Favourite==null)
//            {
//                return NotFound();
//            }
//            // on retourne la confiture
//            return Ok(Favourite);
//        }
//    }

//}


