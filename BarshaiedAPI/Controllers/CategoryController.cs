using BarshaiedAPI.Extensions;
using BusinessLayer.Results;
using BusinessLayer.Services;
using DataAccessLayer.DTOs.CategoryDTOs;
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


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<LightCategoryDTO>>>GetCategories()
        {
            var categories = await _service.GetCategories();
            if (categories == null || !categories.Any())
                return NotFound("No Categories Found");
            return Ok(categories);
        }

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


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AddUpdateServiceResponse<LightCategoryDTO>>>AddCategory(AddCategoryDTO newCategory)
        {
            var categoryResponse = await _service.AddCategory(newCategory);
            return categoryResponse.ToActionResult();
        }


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

        [HttpGet("by-name/{Name}", Name = "GetCategoryByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<LightCategoryDTO>>> GetCategoryByName(string Name)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return BadRequest("Name Can Not Be Null Or WitheSpace");
            }
            var categorie = await _service.GetCategoryByName(Name);
            if (categorie == null || !categorie.Any())
            {
                return NotFound($"No Category Found With Name = {Name}");
            }
            return Ok(categorie);
        }

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
