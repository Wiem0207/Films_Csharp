using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

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


