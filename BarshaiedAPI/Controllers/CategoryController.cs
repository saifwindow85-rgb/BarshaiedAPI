using BarshaiedAPI.Extensions;
using BarshaiedAPI.First_Validations;
using BusinessLayer.Results;
using BusinessLayer.Services;
using DataAccessLayer.DTOs.CategoryDTOs;
using Domain.PagedResult;
using Domain.ReadOnlyModels.CategoryReadOnlyModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BarshaiedAPI.Controllers
{
    [Authorize]
    [Route("api/CategoryController")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private CategoryService _service;
        public CategoryController(CategoryService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PagedResult<LightCategoryDTO>>> GetCategories([FromQuery]PaginationParams @param)
        {
            var categories = await _service.GetCategories(param.PageNumber,param.PageSize);
            return categories.ToPagedActioneResult();
        }

        [AllowAnonymous]
        [HttpGet("{Id}",Name ="GetCategoryById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CategoryDetailsDTO>> GetCategoryById(int Id)
        {
          if(Id < 1)
            {
                return BadRequest($"Not Accepted Id {Id}");
            }
            var categorie = await _service.GetCategoryById(Id);
            if(categorie == null)
            {
                return NotFound($"No Student Found With Id = {Id}");
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
        [HttpDelete("{Id}",Name = "DeleteCategoryById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>>DeleteCategory(int Id)
        {
            if(Id < 1)
            {
                return BadRequest($"Not Accepted Id {Id}");
            }
            var IsDeleted = await _service.Delete(Id);
            if(!IsDeleted)
            {
                return NotFound($"No Category Found With Id = {Id}");
            }
            return Ok($"Category Deleted Succesfuly");
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("{Id}",Name ="UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AddUpdateServiceResponse<LightCategoryDTO>>>UpdateCategory(int Id,UpdateCategoryDTO updateCategoryDTO)
        {
            if( Id < 1)
            {
                return BadRequest($"Not Accepted Id {Id}");
            }
            AddUpdateServiceResponse<LightCategoryDTO> categoryResponse = await _service.UpdateCategory(Id, updateCategoryDTO);
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
       public async Task<ActionResult<List<CategoryDetailsDTO>>>GetCategoriesDetails()
        {
            var categoriesDetails = await _service.GetCategoriesDetails();
            if (categoriesDetails == null || !categoriesDetails.Any())
                return NotFound("No Result Found !");
            return Ok(categoriesDetails);
        }
    }
}
