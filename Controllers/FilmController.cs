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
    [Route("api/film")]
    public class FilmController : ControllerBase
    {
        private readonly BddContext _context;
        public FilmController(BddContext context)
        {
            _context = context;
        }
        //pour recuperer une liste de tt les films
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Film>>> GetFilms()
        {
            var films = await _context.Films.ToListAsync();
            if (films == null || films.Count == 0)
            {
                return NotFound();
            }

            return Ok(films);
        }
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Film>>> SearchFilms([FromQuery] string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return BadRequest("Le mot-clé est requis.");
            }

            var films = await _context.Films.Where(f => f.Name.Contains(keyword)).ToListAsync();
            if (films == null || films.Count == 0)
            {
                return NotFound("Aucun film trouvé pour ce mot-clé.");
            }

            return Ok(films);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Film>> GetFilm(int id)
        {
            var film = await _context.Films.FindAsync(id);

            if (film == null)
            {
                return NotFound("Film introuvable.");
            }

            return Ok(film);
        }
        public class FilmCreation
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Poster { get; set; }
            public string imdb { get; set; }
            public int Year { get; set; }
        }
        [HttpPost]
        public async Task<ActionResult<Film>> PostFilm(FilmCreation filmCreation)
        {
            // on créer une nouvelle confiture avec les informations reçu
            Film film = new Film
            {
                Name = filmCreation.Name,
                Poster = filmCreation.Poster,
                Year = filmCreation.Year,
                imdb = filmCreation.imdb,
            };
            // Hachage du mot de passe avant de le stocker
            _context.Films.Add(film);
            // on enregistre les modifications dans la BDD ce qui remplira le champ Id de notre objet
            await _context.SaveChangesAsync();
            // on retourne un code 201 pour indiquer que la création a bien eu lieu
            return CreatedAtAction(nameof(GetFilm), new { id = film.Id }, film);
        }
        [HttpGet("info")]
        public async Task<ActionResult<IEnumerable<Film>>> GetFilms([FromQuery] int[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return BadRequest("Aucun ID de film fourni.");
            }

            // Rechercher les films dont les IDs sont dans le tableau passé
            var films = await _context.Films
                                      .Where(f => ids.Contains(f.Id))
                                      .ToListAsync();

            // Si aucun film n'est trouvé
            if (films.Count == 0)
            {
                return NotFound("Aucun film trouvé pour les IDs fournis.");
            }

            // Retourner la liste des films trouvés
            var result = films.Select(f => new
            {
                f.Id,
                f.Name,
                f.Year,
                f.Poster // Assurez-vous que 'Poster' est un champ de l'entité Film
            }).ToList();

            return Ok(result);
        }
    }
}


