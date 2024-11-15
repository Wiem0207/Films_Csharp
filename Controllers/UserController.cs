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
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly BddContext _context;
        private readonly PasswordHasher<User> _passwordHasher;

        // Injection du contexte de donn�es et du service PasswordHasher
        public UserController(BddContext context, PasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound("Utilisateur introuvable.");
            }

            return Ok(user);
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
            // on cr�er une nouvelle confiture avec les informations re�u
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
            // on retourne un code 201 pour indiquer que la cr�ation a bien eu lieu
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User userUpdate)
        {
            // on r�cup�re la confiture que l'on souhaite modifier
            User user = await _context.Users.FindAsync(userUpdate.Id);
            if (user == null)
            {
                return NotFound();
            }

            // on met a jour les informations de la confiture
            user.Pseudo = userUpdate.Pseudo;
            user.Password = userUpdate.Password;
            user.Role = userUpdate.Role;
            // on indique a notre contexte que l'objet a �t� modifi�
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
            // on r�cup�re la confiture que l'on souhaite supprimer
            User user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            // on indique a notre contexte que l'objet a �t� supprim�
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
                return Unauthorized("Utilisateur non trouv�");
            }

            // V�rification du mot de passe en le comparant avec le mot de passe hach� stock� en base
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, userInfo.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Mot de passe incorrect");
            }

            // Si l'utilisateur est authentifi�, on retourne l'utilisateur
            return Ok(user);
        }
    }
}


