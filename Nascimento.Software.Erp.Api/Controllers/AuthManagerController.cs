using Domain.Users;
using Infra.DAL.Contracts;
using Microsoft.AspNetCore.Mvc;
using Nascimento.Software.Erp.Api.Dto.AuthResult;
using Nascimento.Software.Erp.Api.Dto.User;
using Services.Jwt;
using Services.Password;

namespace Nascimento.Software.Erp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthManagerController : ControllerBase
    {
        private readonly IUserDAL _userDAL;
        private readonly IJWTService _jwtService;
        private readonly IPasswordService _passwordSvc;
        public AuthManagerController(IUserDAL userDAL,
            IPasswordService _passwordSvc,
            IJWTService _jwtSvcs)
        {
            _jwtService = _jwtSvcs;
            _userDAL = userDAL;
            this._passwordSvc = _passwordSvc;
        }
        [HttpPost]
        [Route("user-login")]
        public async Task<ActionResult> Login(UserLoginDto userLoginDto)
        {
            var authResult = new AuthResultDto();
            var userLogin = new User()
            {
                Email = userLoginDto.Email,
                PasswordHash = _passwordSvc.EncryptPassword(userLoginDto.Password),
            };
            var isLoginSucessfull = await _userDAL.Login(userLogin);
            if (isLoginSucessfull == null)
            {
                authResult.Succesfull = false;
                authResult.Errors.Add("Login unsucessfull");
                return BadRequest(authResult);
            }

            var token = _jwtService.GenerateJwtToken(isLoginSucessfull);
            if (string.IsNullOrWhiteSpace(token))
            {
                authResult.Succesfull = true;
                authResult.Errors.Add("Sorry, your login was sucesfull but we couldn't generate your Access Token!");
                return BadRequest(authResult);
            }
            authResult.Email = isLoginSucessfull.Email;
            authResult.Succesfull = true;
            authResult.Token = token;
            return Ok(authResult);
        }
        [HttpPost]
        [Route("user-register")]
        public async Task<ActionResult> UserRegister(UserRegisterDto userDto)
        {
            var authResult = new AuthResultDto();

            var userLogin = new User()
            {
                Email = userDto.Email,
                CellPhone = userDto.CellPhone,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                UserName = userDto.UserName,
                PasswordHash = _passwordSvc.EncryptPassword(userDto.Password),
            };
            var isCreated = await _userDAL.Register(userLogin);
            if (isCreated == null)
            {
                authResult.Succesfull = false;
                authResult.Errors.Add("There's a problem with your registration, check the data you sent");
                return BadRequest(authResult);
            }

            var token = _jwtService.GenerateJwtToken(isCreated);
            if (string.IsNullOrWhiteSpace(token))
            {
                authResult.Succesfull = true;
                authResult.Errors.Add("Your registration is completed, but we couldn't generate your access token. Try again later");
                return BadRequest(authResult);
            }

            authResult.Succesfull = true;
            authResult.Token = token;
            return Ok(authResult);
        }
    }
}