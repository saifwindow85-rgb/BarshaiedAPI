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
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ProductDTO>>>GetAllProducts()
        {
            var products = await _service.GetAllProducts();
            if(products == null || !products.Any())
            {
                return NotFound("No Products Found");
            }
            return Ok(products);
        }
    }
}
