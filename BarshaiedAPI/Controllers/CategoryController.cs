using BusinessLayer.Services;
using Domain.DTOs.CategoryDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BarshaiedAPI.Controllers
{
    [Route("api/CategoryController")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private CategoryService _service;
        public CategoryController(CategoryService service)
        {
            _service = service;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<CategoryDTO>>>GetCategories()
        {
            var categories = await _service.GetCategories();
            if (categories == null || !categories.Any())
                return NotFound("No Categories Found");
            return Ok(categories);
        }
    }
}
