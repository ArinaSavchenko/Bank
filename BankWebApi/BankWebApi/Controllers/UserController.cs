using System.Threading.Tasks;
using Bank.Datalayer.Entities;
using BankWebApi.DTOs;
using BankWebApi.Helpers;
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

        public UserController(UserService userService)
        {
            _userService = userService;
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
        public async Task<ActionResult> Register([FromBody] RegisterModel model)
        {
            var registrationResult = await _userService.RegisterAsync(model);

            if (registrationResult.Success == false)
            {
                return BadRequest();
            }

            return Ok();
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
    }
}