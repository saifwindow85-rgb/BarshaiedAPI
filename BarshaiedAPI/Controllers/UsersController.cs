using BarshaiedAPI.Extensions;
using BarshaiedAPI.First_Validations;
using Domain.DTOs.UserDTOs;
using Domain.Results;
using BusinessLayer.Services;
using Domain.PagedResult;
using Domain.ReadOnlyModels.UserDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Domain.Interfaces.Services_Interfaces;

namespace BarshaiedAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;
        public UsersController(IUserService service)
        {
            _service = service;
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("all",Name ="GetAllUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<PagedResult<UserDTO>>> GetAllUsers([FromQuery]PaginationParams @param)
        {
            var users = await _service.GetAllUsers(param.PageNumber, param.PageSize);
            return users.ToPagedActioneResult();
        }

        [Authorize(Roles = "Admin,User,Viewer")]
        [HttpGet("By-Id",Name ="GetUserById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<UserDTO>> GetUserById([FromQuery]IdInputValidator @param,
             [FromServices] IAuthorizationService authorizationService)
        {
            var user = await _service.GetUserById(param.Id);


            if(user == null)
            {
                return NotFound($"No User Found With Id = {param.Id}");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            var authResult = await authorizationService.AuthorizeAsync(User, param.Id, "UserOwnerOrAdmin");
            if(!authResult.Succeeded)
                return NotFound($"No User Found With Id = {param.Id}");

            return Ok(user);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<AddUpdateServiceResponse<UserDTO>>>AddUser(AddUserDTO newUser)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
            var response = await _service.AddUser(newUser,userId);
                return response.ToActionResult();
        }
    }
}
