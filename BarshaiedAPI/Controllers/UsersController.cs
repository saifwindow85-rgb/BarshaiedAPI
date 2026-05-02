using BarshaiedAPI.Extensions;
using BarshaiedAPI.First_Validations;
using BusinessLayer.AddUpdateDTOs.UserDTOs;
using BusinessLayer.Results;
using BusinessLayer.Services;
using Domain.PagedResult;
using Domain.ReadOnlyModels.UserDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BarshaiedAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _service;
        public UsersController(UserService service)
        {
            _service = service;
        }

        [Authorize(Roles ="Admin,User")]
        [HttpGet("all",Name ="GetAllUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<PagedResult<UserDTO>>> GetAllUsers([FromQuery]PaginationParams @param)
        {
            var users = await _service.GetAllUsers(param.PageNumber, param.PageSize);
            return users.ToPagedActioneResult();
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("By-Id",Name ="GetUserById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<UserDTO>> GetUserById([FromQuery]IdInputValidator @param)
        {
            var user = await _service.GetUserById(param.Id);
            if(user == null)
            {
                return NotFound($"No User Found With Id = {param.Id}");
            }
            return Ok(user);
        }


        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<AddUpdateServiceResponse<UserDTO>>>AddUser(AddUserDTO newUser)
        {
            var response = await _service.AddUser(newUser);
            return response.ToActionResult();
        }
    }
}
