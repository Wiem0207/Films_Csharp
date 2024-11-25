using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
        [ApiController]
        [Route("api/Favourite")]
        public class FavouriteController : ControllerBase
        {
            private readonly BddContext _context;

            public FavouriteController(BddContext context)
            {
                _context = context;
            }

            // POST api/Favourite/add
            [HttpPost("add")]
            public async Task<ActionResult> AddFavourite([FromBody] int filmId)
            {
            int userId = 1;
             
                // Vérifier si le film existe
                var film = await _context.Films.FindAsync(filmId);
                if (film == null)
                {
                    return NotFound("Film non trouvé.");
                }

                // Vérifier si le film est déjà dans les favoris de l'utilisateur
                var existingFavourite = await _context.Favourites
                    .FirstOrDefaultAsync(f => f.UserId == userId && f.FilmId == filmId);
                if (existingFavourite != null)
                {
                    return Conflict("Ce film est déjà dans les favoris.");
                }
            int nextId = (_context.Favourites.Any())
             ? _context.Favourites.Max(f => f.Id) + 1
             : 1;

            // Ajouter aux favoris
            var favourite = new Favourite
                {
                    UserId = userId,
                    FilmId = filmId,
                    Id= nextId
                };

                _context.Favourites.Add(favourite);
                await _context.SaveChangesAsync();

                return Ok("Film ajouté aux favoris avec succès.");
            }
        }

}

