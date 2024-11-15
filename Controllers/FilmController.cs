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
//public class UserCreation
//{
//    public string Pseudo { get; set; }
//    public string Password { get; set; }
//    public Role Role { get; set; }
//}
//[HttpPost]
//public async Task<ActionResult<User>> PostUser(UserCreation userCreation)
//{
//    // on créer une nouvelle confiture avec les informations reçu
//    User user = new User
//    {
//        Pseudo = userCreation.Pseudo,
//        Role = userCreation.Role
//    };
//    // Hachage du mot de passe avant de le stocker
//    user.Password = _passwordHasher.HashPassword(user, userCreation.Password);
//    // on l'ajoute a notre contexte (BDD)
//    _context.Users.Add(user);
//    // on enregistre les modifications dans la BDD ce qui remplira le champ Id de notre objet
//    await _context.SaveChangesAsync();
//    // on retourne un code 201 pour indiquer que la création a bien eu lieu
//    return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
//}
//[HttpPut("{id}")]
//public async Task<IActionResult> PutUser(int id, User userUpdate)
//{
//    // on récupère la confiture que l'on souhaite modifier
//    User user = await _context.Users.FindAsync(userUpdate.Id);
//    if (user == null)
//    {
//        return NotFound();
//    }

//    // on met a jour les informations de la confiture
//    user.Pseudo = userUpdate.Pseudo;
//    user.Password = userUpdate.Password;
//    user.Role = userUpdate.Role;
//    // on indique a notre contexte que l'objet a été modifié
//    _context.Entry(user).State = EntityState.Modified;

//    try
//    {
//        // on enregistre les modifications
//        await _context.SaveChangesAsync();
//    }
//    catch (DbUpdateConcurrencyException)
//    {
//        // si une erreur de concurrence survient on retourne un code 500
//        return StatusCode(500, "Erreur de concurrence");
//    }
//    // on retourne un code 200 pour indiquer que la modification a bien eu lieu
//    return Ok(user);
//}
//[HttpDelete("{id}")]
//public async Task<IActionResult> DeleteUser(int id)
//{
//    // on récupère la confiture que l'on souhaite supprimer
//    User user = await _context.Users.FindAsync(id);
//    if (user == null)
//    {
//        return NotFound();
//    }
//    // on indique a notre contexte que l'objet a été supprimé
//    _context.Users.Remove(user);
//    // on enregistre les modifications
//    await _context.SaveChangesAsync();
//    // on retourne un code 204 pour indiquer que la suppression a bien eu lieu
//    return NoContent();
//}
//[HttpPost("login")]
//public async Task<ActionResult<User>> Login([FromBody] UserInfo userInfo)
//{
//    // Recherche de l'utilisateur avec le pseudo
//    var user = await _context.Users.SingleOrDefaultAsync(u => u.Pseudo == userInfo.Pseudo);
//    if (user == null)
//    {
//        return Unauthorized("Utilisateur non trouvé");
//    }

//    // Vérification du mot de passe en le comparant avec le mot de passe haché stocké en base
//    var result = _passwordHasher.VerifyHashedPassword(user, user.Password, userInfo.Password);
//    if (result == PasswordVerificationResult.Failed)
//    {
//        return Unauthorized("Mot de passe incorrect");
//    }

//    // Si l'utilisateur est authentifié, on retourne l'utilisateur
//    return Ok(user);
//}

//    }
//    ///////////////

//}


