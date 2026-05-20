using BarshaiedAPI.Extensions;
using BarshaiedAPI.First_Validations;
using BusinessLayer.Services;
using Domain.DTOs.ShoppingListPageDTOs;
using Domain.Interfaces.Services_Interfaces;
using Domain.PagedResult;
using Domain.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace BarshaiedAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingListPageController : ControllerBase
    {
        private readonly IShoppingListPageService _service;
        public ShoppingListPageController(IShoppingListPageService service)
        {
            _service = service;
        }
        [Authorize(Roles = "Admin,User")]
        [EnableRateLimiting("UserSlidingLimiter")]
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PagedResult<ShoppingListPageReadOnlyDTO>>>GetShoppingListPages([FromQuery]PaginationParams @param)
        {
            var pages = await _service.GetShoppingListPage(param.PageNumber, @param.PageSize);
            return pages.ToPagedActioneResult();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        [EnableRateLimiting("UserSlidingLimiter")]
        [ProducesResponseType(typeof(ShoppingListPageReadOnlyDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AddUpdateServiceResponse<ShoppingListPageReadOnlyDTO>>> AddShoppingListPage(
    [FromBody] AddShoppingListPageDTO dto)
        {
            // استخراج UserId من الـ JWT Token
            var creatorId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var result = await _service.AddShoppingListPage(dto, creatorId);

            return result.ToActionResult();
        }
    }
}
