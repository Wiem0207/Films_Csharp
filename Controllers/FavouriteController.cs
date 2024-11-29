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
        public async Task<ActionResult> AddFavourite(int filmId)
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
                Id = nextId
            };

            _context.Favourites.Add(favourite);
            await _context.SaveChangesAsync();

            return Ok("Film ajouté aux favoris avec succès.");
        }
        [HttpDelete("{filmId}")]
        public async Task<IActionResult> Deletefavourite(int filmId)
        {
            int userId = 1;
            // on récupère la confiture que l'on souhaite supprimer
            Favourite favourite = await _context.Favourites.FirstOrDefaultAsync(f => f.UserId == userId && f.FilmId == filmId);
            if (favourite == null)
            {
                return NotFound($"Le film avec l'ID {filmId} n'est pas dans les favoris de l'utilisateur {userId}.");
            }
            // on indique a notre contexte que l'objet a été supprimé
            _context.Favourites.Remove(favourite);
            // on enregistre les modifications
            await _context.SaveChangesAsync();
            // on retourne un code 204 pour indiquer que la suppression a bien eu lieu
            return Ok($"Le film avec l'ID {filmId} a été supprimé des favoris de l'utilisateur {userId}.");
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Favourite>>> GetFavourites()
        {
            int userID = 1;
            var favouriteFilmIds = await _context.Favourites
            .Where(f => f.UserId == userID)  // Filtrer par l'utilisateur
            .Select(f => f.FilmId)  // Sélectionner uniquement les FilmId
            .ToListAsync();

            if (favouriteFilmIds == null || favouriteFilmIds.Count == 0)
            {
                return NotFound("Aucun film trouvé dans les favoris.");
            }

            return Ok(favouriteFilmIds);
        }
    }
}
