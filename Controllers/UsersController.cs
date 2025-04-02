using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BIBLIOTECA_API.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BIBLIOTECA_API.Controllers
{
    [Route("api/usuarios")]
    [Authorize]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly string _keyJwt;

        public UsersController(UserManager<IdentityUser> userManager,IConfiguration configuration,SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager;
            _keyJwt = Environment.GetEnvironmentVariable("key_jwt")!;
        }

        [HttpPost("registro")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthenticationResponseDTO>> Register(CredentialsUsersDTO credentialsUsersDto)
        {
            var user = new IdentityUser
            {
                UserName = credentialsUsersDto.Email,
                Email = credentialsUsersDto.Email
            };
            var result = await _userManager.CreateAsync(user, credentialsUsersDto.Password!);
            if (result.Succeeded)
            {
                var responseAuthentication = await CreateToken(credentialsUsersDto);
                return responseAuthentication;
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty,error.Description);
                }

                return ValidationProblem();
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthenticationResponseDTO>> login(CredentialsUsersDTO credentialsUsersDto)
        {
            //Buscar al usuario por email
            var user = await _userManager.FindByEmailAsync(credentialsUsersDto.Email);
            if (user is null)
            {
                return ReturnlLoginFailed();
            }

            var result =
                await _signInManager.CheckPasswordSignInAsync(user, credentialsUsersDto.Password!,
                    // Si el usuario se equivoca varias veces no se va a bloquear la cuenta
                    lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return await CreateToken(credentialsUsersDto);
            }
            else
            {
                return ReturnlLoginFailed();
            }
        }

        private ActionResult ReturnlLoginFailed()
        {
            ModelState.AddModelError(string.Empty,"Login Incorrecto");
            return ValidationProblem();
        }
        private async Task<AuthenticationResponseDTO> CreateToken(CredentialsUsersDTO credentialsUsersDto)
        {
            // Crear los claims al usuario
            var claims = new List<Claim>
            {
                new Claim("email", credentialsUsersDto.Email),
                new Claim("cualquier cosa", "cualquier valor")
            };
            // Buscar al usuario en base al Email
            var usuario = await _userManager.FindByEmailAsync(credentialsUsersDto.Email);
            // Traer los claims del usuario de la DB
            var claimsDB = await _userManager.GetClaimsAsync(usuario!);
            // Agregar los Claims DB
            claims.AddRange(claimsDB);
            
            //LLave secreta 

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_keyJwt));
            // Crear la firma
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            // Expiracion
            // var expire = DateTime.UtcNow.AddDays(1);
            var expire = DateTime.UtcNow.AddYears(1);
            var tokenSecutiry = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expire,
                signingCredentials: credentials);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenSecutiry);
            return new AuthenticationResponseDTO
            {
                Token = token,
                Expiracion = expire
            };
        }
    }
}
