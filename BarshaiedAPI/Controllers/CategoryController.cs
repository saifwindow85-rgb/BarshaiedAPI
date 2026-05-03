using BarshaiedAPI.Extensions;
using BarshaiedAPI.First_Validations;
using Domain.Results;
using BusinessLayer.Services;
using Domain.DTOs.CategoryDTOs;
using Domain.PagedResult;
using Domain.ReadOnlyModels.CategoryReadOnlyModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.Interfaces.Services_Interfaces;

namespace BarshaiedAPI.Controllers
{
    [Authorize]
    [Route("api/CategoryController")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private ICategoryServices _service;
        public CategoryController(ICategoryServices service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PagedResult<LightCategoryDTO>>> GetCategories([FromQuery]PaginationParams @param)
        {
            var categories = await _service.GetCategories(param.PageNumber,param.PageSize);
            return categories.ToPagedActioneResult();
        }

        [AllowAnonymous]
        [HttpGet("By-Id",Name ="GetCategoryById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CategoryDetailsDTO>> GetCategoryById([FromQuery] IdInputValidator @param)
        {
            var categorie = await _service.GetCategoryById(param.Id);
            if(categorie == null)
            {
                return NotFound($"No Student Found With Id = {param.Id}");
            }
            return Ok(categorie);
        }

        [Authorize(Roles ="Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<AddUpdateServiceResponse<LightCategoryDTO>>>AddCategory(AddCategoryDTO newCategory)
        {
            var categoryResponse = await _service.AddCategory(newCategory);
            return categoryResponse.ToActionResult();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("",Name = "DeleteCategoryById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>>DeleteCategory([FromQuery] IdInputValidator @param)
        {
            var IsDeleted = await _service.Delete(param.Id);
            if(!IsDeleted)
            {
                return NotFound($"No Category Found With Id = {param.Id}");
            }
            return Ok($"Category Deleted Succesfuly");
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("",Name ="UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AddUpdateServiceResponse<LightCategoryDTO>>> UpdateCategory([FromQuery]IdInputValidator @param,UpdateCategoryDTO updateCategoryDTO)
        {
            
            AddUpdateServiceResponse<LightCategoryDTO> categoryResponse = await _service.UpdateCategory(param.Id, updateCategoryDTO);
            return categoryResponse.ToActionResult();
        }

        [AllowAnonymous]
        [HttpGet("by-name/{Name}", Name = "GetCategoryByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PagedResult<LightCategoryDTO>>> GetCategoryByName(string Name, [FromQuery]PaginationParams @param)
        {
            var categories = await _service.GetCategoryByName(Name, param.PageNumber, param.PageSize);
            return categories.ToPagedActioneResult();
        }

        [Authorize(Roles ="Admin,User")]
        [HttpGet("by-category", Name = "GetCategoriesDetails")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
       public async Task<ActionResult<PagedResult<CategoryReportDTO>>> GetCategoriesDetails([FromQuery]PaginationParams @param)
        {
            var categoriesDetails = await _service.GetCategoriesDetails(param.PageNumber,param.PageSize);
            return categoriesDetails.ToPagedActioneResult();
        }
    }
}
