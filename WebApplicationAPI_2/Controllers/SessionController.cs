using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplicationAPI_2.DTO;
using WebApplicationAPI_2.Modelos;
using WebApplicationAPI_2.Repos;

namespace WebApplicationAPI_2.Controllers
{
    [Route("Session")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<SessionController> _logger;
        private readonly IUserSQL repositorio;

        public SessionController(IConfiguration configuration, ILogger<SessionController> logger, IUserSQL repositorio)
        {
            _configuration = configuration;
            _logger = logger;
            this.repositorio = repositorio;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<UserDTO>> AddProduct(UserSession p)
        {
            User person = null;

            person = await AutenticateUserAsync(p);
            if (person == null)
            {
                throw new Exception("Incorrect Credentials");
            } else
            {
                person = MakeTokenJWT(person);
            }

            return person.convertTransform();
        }

        private async Task<User> AutenticateUserAsync(UserSession userLogin)
        {
            User u = await repositorio.LoginUser(userLogin);
            return u;
        }

        private User MakeTokenJWT(User usuarioInfo)
        {
            // Cabecera
            var _symmetricSecurityKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["JWT:ClaveSecreta"])
                );
            var _signingCredentials = new SigningCredentials(
                    _symmetricSecurityKey, SecurityAlgorithms.HmacSha256
                );
            var _Header = new JwtHeader(_signingCredentials);

            // Claims
            var _Claims = new[] {
                new Claim("idUser", usuarioInfo.userId),
                new Claim("usuario", usuarioInfo.nameUser),
                new Claim("email", usuarioInfo.emailUser),
                new Claim(JwtRegisteredClaimNames.Email, usuarioInfo.emailUser),
            };

            //Payload
            var _Payload = new JwtPayload(
                    issuer: _configuration["JWT:Issuer"],
                    audience: _configuration["JWT:Audience"],
                    claims: _Claims,
                    notBefore: DateTime.UtcNow,
                     expires: DateTime.UtcNow.AddHours(1)
                );

            // Token
            var _Token = new JwtSecurityToken(
                    _Header,
                    _Payload
                );
            usuarioInfo.tokenUser = new JwtSecurityTokenHandler().WriteToken(_Token);

            return usuarioInfo;
        }
    }
}
