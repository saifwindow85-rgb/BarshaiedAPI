using BarshaiedAPI.Extensions;
using BarshaiedAPI.First_Validations;
using BusinessLayer.DTOs.ProductDTOs;
using BusinessLayer.Results;
using BusinessLayer.Services;
using Domain.PagedResult;
using Domain.ReadOnlyModels.Product_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BarshaiedAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private ProductService _service;
        public ProductController(ProductService service)
        {
            _service = service;
        }
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PagedResult<ReadOnlyProductDTO>>> GetAllProducts([FromQuery]PaginationParams @params)
        {

            var products = await _service.GetAllProducts(@params.PageNumber, @params.PageSize);
            return products.ToPagedActioneResult();
        }

        [HttpGet("by-Id{Id}",Name = "GetProductById")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
         public async Task<ActionResult<DetailedProductDTO>> GetProductById([FromQuery]IdInputValidator @param)
        {
         
            var productDetails = await _service.GetProductById(param.Id);
            if(productDetails == null)
            {
                return NotFound($"No Product Found With Id = {param.Id}");
            }
            return Ok(productDetails);
        }

        [HttpDelete("{Id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>>DeleteProduct([FromQuery] IdInputValidator @param)
        {
           
            var IsDeleted = await _service.Delete(param.Id);
            if(!IsDeleted)
            {
                return NotFound($"No Product Found With Id = {param.Id}");
            }
            return Ok($"Product With Id = {param.Id} Deleted Succesfully");
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<AddUpdateServiceResponse<DetailedProductDTO>>> AddProduct(AddProductDTO newProduct)
        {

            var postResponse = await _service.AddProduct(newProduct);
            return postResponse.ToActionResult();
        }

        [HttpGet("get-expired-products{pageNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PagedResult<ReadOnlyProductDTO>>> GetExpiredProducts([FromQuery]PaginationParams @param)
        {
          
            var ExpierdProducts = await _service.GetExpiredProducts(param.PageNumber,param.PageSize);
            return ExpierdProducts.ToPagedActioneResult();
        }

        [HttpGet("get-zero-Quantity-products{pageNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ReadOnlyProductDTO>>> GetZeroQuantityProducts(int pageNumber)
        {
            if(pageNumber < 1)
            {
                return BadRequest($"Not Accepted PageNumber !{pageNumber}");
            }
            var zeroQuantityProducts = await _service.GetZeroQuantityProducts(pageNumber);
            if (zeroQuantityProducts == null || !zeroQuantityProducts.Any())
            {
                return NotFound("No Products With Zero Quantity Found");
            }
            return Ok(zeroQuantityProducts);
        }


        [HttpGet("get-under-min-quantity{pageNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ReadOnlyProductDTO>>>GetProductsUnderMinQuantity(int pageNumber)
        {
            if (pageNumber < 1)
            {
                return BadRequest($"Not Accepted PageNumber !{pageNumber}");
            }
            var products = await _service.GetProductsUnderMinQuantity(pageNumber);
            if(products == null || ! products.Any())
            {
                return NotFound("No Products Under The Min Quantity Found !");
            }
            return Ok(products);
        }

        [HttpGet("get-by-name-barcode{nameOrBarcode}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PagedResult<ReadOnlyProductDTO>>> GetProductsByNameOrBarcode([FromQuery]PaginationParams @params,string nameOrBarcode)
        {
        
            if(string.IsNullOrWhiteSpace(nameOrBarcode))
            {
                return BadRequest("Name Or Barcode Can Not Be Null!");
            }
            var products = await _service.GetProductByNameOrBarcode(@params.PageNumber,@params.PageSize,nameOrBarcode);
           return  products.ToPagedActioneResult();
        }

        [HttpGet("get-nearing-expiry-products{pageNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ReadOnlyProductDTO>>> GetNearingExpiryProduct(int pageNumber)
        {
            if (pageNumber < 1)
            {
                return BadRequest($"Not Accepted PageNumber !{pageNumber}");
            }
            var products = await _service.ProductsNearingExpiry(pageNumber);
            if (products == null || !products.Any())
            {
                return NotFound("No Products Under The Min Quantity Found !");
            }
            return Ok(products);
        }


        [HttpPut("{Id}",Name ="UpdateProduct")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
         public async Task<ActionResult<AddUpdateServiceResponse<DetailedProductDTO>>> UpdateProduct([FromQuery] IdInputValidator @param, UpdateProductDTO updatedProduct)
        {
          
            var updateResponse = await _service.UpdateProduct(param.Id, updatedProduct);
            return updateResponse.ToActionResult();
        }
    }
}
