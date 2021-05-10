using System.Threading.Tasks;
using Bank.Datalayer.Entities;
using BankWebApi.DTOs;
using BankWebApi.Helpers;
using BankWebApi.Models;
using BankWebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankWebApi.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly JwtService _jwtService;

        public UserController(UserService userService, JwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }
        
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetUsers([FromQuery] UserForSearchModel model)
        {
            var users = await _userService.GetUsers(model);

            return Ok(users);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPut("update/{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateUserModel model)
        {
            var updateResult = await _userService.UpdateUserAsync(id, model);

            if (updateResult.Success == false)
            {
                return BadRequest();
            }

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<Response<string>>> Register([FromBody] RegisterModel model)
        {
            var registrationResult = await _userService.RegisterAsync(model);

            return Ok(registrationResult);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var deleteResult = await _userService.DeleteUserAsync(id);

            if (deleteResult.Success == false)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpGet("clients/{id}")]
        public async Task<ActionResult> GetClient(int id)
        {
            var user = await _userService.GetClientById(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPut("update/clients")]
        public async Task<ActionResult> UpdateClient([FromBody] ClientViewModel model)
        {
            var updateResult = await _userService.UpdateClientAsync(model);

            if (updateResult.Success == false)
            {
                return BadRequest();
            }

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult> Authenticate([FromBody] AuthenticateModel model)
        {
            Response<User> authorizationResult = await _userService.AuthenticateAsync(model);

            if (authorizationResult.Success == false)
            {
                return BadRequest(authorizationResult);
            }

            string tokenString = _jwtService.GenerateJwtToken(authorizationResult.Data);

            var result = new Response<string>
            {
                Message = authorizationResult.Message,
                Data = tokenString
            };

            return Ok(result);
        }
    }
}