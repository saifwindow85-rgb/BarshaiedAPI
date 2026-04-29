using BarshaiedAPI.Extensions;
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


        [HttpGet("{pageNumber}/{pageSize}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PagedResult<UserDTO>>>GetAllUsers(int pageNumber,int pageSize)
        {
            var users = await _service.GetAllUsers(pageNumber, pageSize);
            if(users.Data == null || !users.Data.Any())
            {
                return NotFound("No Result Found");
            }
            return Ok(users);
        }

        [HttpGet("{Id}",Name ="GetUserById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDTO>>GetUserById(int Id)
        {
            var user = await _service.GetUserById(Id);
            if(user == null)
            {
                return NotFound($"No User Found With Id = {Id}");
            }
            return Ok(user);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<AddUpdateServiceResponse<UserDTO>>>AddUser(AddUserDTO newUser)
        {
            var response = await _service.AddUser(newUser);
            return response.ToActionResult();
        }
    }
}
