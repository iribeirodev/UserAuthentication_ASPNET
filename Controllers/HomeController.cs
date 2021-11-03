using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopAutenticacao.Request;
using ShopAutenticacao.Response;
using ShopAutenticacao.Services;

namespace ShopAutenticacao.Controllers
{
    [Route("v1/account")]
    [ApiController]
    public class HomeController: ControllerBase
    {
        private readonly IUserService _userService;
        public HomeController(IUserService userService) 
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("anonymous")]
        [AllowAnonymous]
        public string Anonymous() => "Anonimo";

        [HttpGet]
        [Route("authenticated")]
        [Authorize]
        public string Authenticated() =>
            String.Format("Autenticado - {0}", User.Identity.Name);

        [HttpGet]
        [Route("employee")]
        [Authorize(Roles = "employee,manager")]
        public string Employee() => "Funcionario";

        [HttpGet]
        [Route("manager")]
        [Authorize(Roles = "manager")]
        public string Manager() => "Gerente";   

        [HttpPost]
        [Route("register")]
        [Authorize(Roles = "manager")]
        public ActionResult<dynamic> Register([FromBody] CreateUserRequest createUserRequest)
        {
            var user = _userService.GetByUsername(createUserRequest.Username);
            if (user != null)
                return new ObjectResult(new {message = "Usuário existente"}) {
                    StatusCode = 500
                };

            _userService.Create(createUserRequest);

            return Ok(new UserCreatedResponse{
                UserName = createUserRequest.Username,
                Role = createUserRequest.Role
            });
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public ActionResult<dynamic> Authenticate([FromBody] LoginRequest loginRequest) 
        {
            var user =  _userService.GetByUsernameAndPassword(loginRequest.Username, loginRequest.Password);
            if (user == null) 
                return NotFound(new {
                    message = "Usuário/Senha inválidos"
                });

            var token = TokenService.GenerateToken(user);
            if (String.IsNullOrEmpty(token)) 
                return new ForbidResult();

            return new SignedInResponse {
                Username = user.Username,
                Role = user.Role,
                Token = token
            };
        }
    }
}