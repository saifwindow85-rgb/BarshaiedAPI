using BusinessLayer.DTOs.ProductDTOs;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BarshaiedAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private ProductService _service;
        public ProductController(ProductService service)
        {
            _service = service;
        }
        [HttpGet("{pageNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ProductDTO>>>GetAllProducts(int pageNumber)
        {
            var products = await _service.GetAllProducts(pageNumber);
            if(products == null || !products.Any())
            {
                return NotFound("No Products Found");
            }
            return Ok(products);
        }

        [HttpGet("by-Id{Id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
         public async Task<ActionResult<ProductDetailsDTO>>GetProductById(int Id)
        {
            if(Id < 1)
            {
                return BadRequest($"Not Accepted Id {Id}");
            }
            var productDetails = await _service.GetProductById(Id);
            if(productDetails == null)
            {
                return NotFound($"No Product Found With Id = {Id}");
            }
            return Ok(productDetails);
        }

        [HttpDelete("{Id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>>DeleteProduct(int Id)
        {
            if(Id < 1)
            {
                return BadRequest($"Not Accepted Id {Id}");
            }
            var IsDeleted = await _service.Delete(Id);
            if(!IsDeleted)
            {
                return NotFound($"No Product Found With Id = {Id}");
            }
            return Ok($"Product With Id = {Id} Deleted Succesfully");
        }
    }
}
