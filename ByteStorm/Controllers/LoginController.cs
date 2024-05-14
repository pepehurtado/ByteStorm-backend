using ByteStorm.Constants;
using ByteStorm.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ByteStorm.Controllers
{
    [Route("api/LoginController")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        public LoginController(IConfiguration config)
        {
            _config = config;
        }


        [HttpGet]
        public IActionResult Get()
        {
            var currentUser = GetCurrentUser();
            return Ok(currentUser);
        }
        [HttpPost]
        public IActionResult Login(LoginUser userLogin)
        {
            var user = Authenticate(userLogin);
            if (user != null)
            {
                //Crea el token
                var token = Generate(user);
                return Ok(token);
            }
            return NotFound("usuario no encontrado");
        }
        private UserModel Authenticate(LoginUser userLogin)
        {
            var currentUser = UserConstants.Users.FirstOrDefault(user => user.Username.ToLower() == userLogin.UserName.ToLower()
            && user.Password == userLogin.Password);
            if (currentUser != null)
            {
                return currentUser;
            }
            return null;
        }

        private string Generate(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            //Crea los claims

            var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Username),
                    new Claim(ClaimTypes.Role, user.Rol),
                    new Claim(ClaimTypes.Name, user.Fullname),  // Agregando el nombre completo como claim
                    new Claim(ClaimTypes.Email, user.Correo),

                };
            //Crea el token
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddDays(2),
                signingCredentials: credentials) ;
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private UserModel GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                // Obtener los claims del usuario
                var usernameClaim = identity.FindFirst(ClaimTypes.NameIdentifier);
                var roleClaim = identity.FindFirst(ClaimTypes.Role);
                var fullnameClaim = identity.FindFirst(ClaimTypes.Name);  
                var correoClaim = identity.FindFirst(ClaimTypes.Email);   

                if (usernameClaim != null && roleClaim != null)
                {
                    // Crear un UserModel basado en los claims
                    var currentUser = new UserModel
                    {
                        Username = usernameClaim.Value,
                        Rol = roleClaim.Value,
                        
                        // Puedes agregar más propiedades según sea necesario
                    };

                    return currentUser;
                }
            }

            return null;
        }
    }
}
