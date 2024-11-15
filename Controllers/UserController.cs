using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
namespace WebApi.Models
{
    public class UserInfo
    {
        public string Pseudo { get; set; }
        public string Password { get; set; }
    }/////dans models
}

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly UserContext _context;
        private readonly PasswordHasher<User> _passwordHasher;  // Utilisation directe de PasswordHasher<User>

        // Injection du contexte de données dans le constructeur
        public UserController(UserContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();  // Instanciation de PasswordHasher<User>
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            // on récupère la confiture correspondant a l'id
            var User = await _context.Users.FindAsync(id);

            if (User == null)
            {
                return NotFound();
            }
            // on retourne la confiture
            return Ok(User);
        }
        public class UserCreation
        {
            public string Pseudo { get; set; }
            public string Password { get; set; }
            public Role Role { get; set; }
        }
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(UserCreation userCreation)
        {
        // on créer une nouvelle confiture avec les informations reçu
        User user = new User
            {
                Pseudo = userCreation.Pseudo,
                Role = userCreation.Role
            };
            // Hachage du mot de passe avant de le stocker
            user.Password = _passwordHasher.HashPassword(user, userCreation.Password);
            // on l'ajoute a notre contexte (BDD)
            _context.Users.Add(user);
            // on enregistre les modifications dans la BDD ce qui remplira le champ Id de notre objet
            await _context.SaveChangesAsync();
            // on retourne un code 201 pour indiquer que la création a bien eu lieu
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User userUpdate)
        {
            // on récupère la confiture que l'on souhaite modifier
            User user = await _context.Users.FindAsync(userUpdate.Id);
            if (user == null)
            {
                return NotFound();
            }

            // on met a jour les informations de la confiture
            user.Pseudo = userUpdate.Pseudo;
            user.Password = userUpdate.Password;
            user.Role = userUpdate.Role;
            // on indique a notre contexte que l'objet a été modifié
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                // on enregistre les modifications
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // si une erreur de concurrence survient on retourne un code 500
                return StatusCode(500, "Erreur de concurrence");
            }
            // on retourne un code 200 pour indiquer que la modification a bien eu lieu
            return Ok(user);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            // on récupère la confiture que l'on souhaite supprimer
            User user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            // on indique a notre contexte que l'objet a été supprimé
            _context.Users.Remove(user);
            // on enregistre les modifications
            await _context.SaveChangesAsync();
            // on retourne un code 204 pour indiquer que la suppression a bien eu lieu
            return NoContent();
        }
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login([FromBody] UserInfo userInfo)
        {
            // Recherche de l'utilisateur avec le pseudo
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Pseudo == userInfo.Pseudo);
            if (user == null)
            {
                return Unauthorized("Utilisateur non trouvé");
            }

            // Vérification du mot de passe en le comparant avec le mot de passe haché stocké en base
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, userInfo.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Mot de passe incorrect");
            }

            // Si l'utilisateur est authentifié, on retourne l'utilisateur
            return Ok(user);
        }

    }
    ///////////////

}


