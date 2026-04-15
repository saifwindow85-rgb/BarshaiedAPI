using BusinessLayer.Results;
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

        [HttpGet("{Id}",Name ="GetCategoryById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<CategoryDTO>>> GetCategoryById(int Id)
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
        public async Task<ActionResult<CategoryDTO>>AddCategory(AddUpdateCategoryDTO newCategory)
        {
            var categoryResponse = await _service.AddCategory(newCategory);
            if(!categoryResponse.IsSuccess)
            {
                return BadRequest(new
                {
                    Message = categoryResponse.ErrorType.ToString(),
                    Errors = categoryResponse.Errors
                });
            }
            return CreatedAtRoute("GetCategoryById", new { Id = categoryResponse.Data!.Id },categoryResponse.Data);
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
            return Ok();
        }

        [HttpPut("{Id}",Name ="UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CategoryDTO>>UpdateCategory(int Id,AddUpdateCategoryDTO updateCategoryDTO)
        {
            if( Id < 1)
            {
                return BadRequest($"Not Accepted Id {Id}");
            }
            AddUpdateServiceResponse<CategoryDTO> categoryResponse = await _service.UpdateCategory(Id, updateCategoryDTO);
            if(categoryResponse.Data == null)
            {
                return NotFound(new
                {
                    Message = categoryResponse.ErrorType.ToString(),
                    Errors = categoryResponse.Errors
                });
            }
            if(!categoryResponse.IsSuccess)
            {
                if(categoryResponse.ErrorType == BusinessLayer.Enums.EnErrorTypes.NotFound)
                {
                    return NotFound(new
                    {
                        Message = categoryResponse.ErrorType.ToString(),
                        Errors = categoryResponse.Errors
                    });
                }
                else if(categoryResponse.ErrorType == BusinessLayer.Enums.EnErrorTypes.InvalidData)
                {
                    return BadRequest(new
                    {
                        Message = categoryResponse.ErrorType.ToString(),
                        Errors = categoryResponse.Errors
                    });
                }
            }
                return Ok(categoryResponse.Data);
        }

        [HttpGet("by-name/{Name}", Name = "GetCategoryByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<CategoryDTO>>> GetCategoryById(string Name)
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
    }
}
