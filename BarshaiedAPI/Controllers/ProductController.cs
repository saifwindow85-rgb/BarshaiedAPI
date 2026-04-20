using BusinessLayer.DTOs.ProductDTOs;
using BusinessLayer.Results;
using BusinessLayer.Services;
using Domain.ReadOnlyModels.Product_Models;
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
        public async Task<ActionResult<List<LightProductObject>>>GetAllProducts(int pageNumber)
        {
            if (pageNumber < 1)
            {
                return BadRequest($"Not Accepted PageNumber !{pageNumber}");
            }
            var products = await _service.GetAllProducts(pageNumber);
            if(products == null || !products.Any())
            {
                return NotFound("No Products Found");
            }
            return Ok(products);
        }

        [HttpGet("by-Id{Id}",Name = "GetProductById")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
         public async Task<ActionResult<DetailedProductObject>>GetProductById(int Id)
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<AddUpdateServiceResponse<DetailedProductObject>>>AddProduct(AddUpdateProductDTO newProduct)
        {

            var postResponse = await _service.AddProduct(newProduct);
            if(!postResponse.IsSuccess)
            {
                return BadRequest(new
                {
                    Message = postResponse.ErrorType.ToString(),
                    Errors = postResponse.Errors
                });
            }
            return CreatedAtRoute("GetProductById", new { Id = postResponse.Data!.ProductId}, postResponse.Data);
        }

        [HttpGet("get-expired-products{pageNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<LightProductObject>>> GetExpiredProducts(int pageNumber)
        {
            if (pageNumber < 1)
            {
                return BadRequest($"Not Accepted PageNumber !{pageNumber}");
            }
            var ExpierdProducts = await _service.GetExpiredProducts(pageNumber);
            if (ExpierdProducts == null || !ExpierdProducts.Any())
            {
                return NotFound("No Products Found");
            }
            return Ok(ExpierdProducts);
        }

        [HttpGet("get-zero-Quantity-products{pageNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<LightProductObject>>> GetZeroQuantityProducts(int pageNumber)
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
        public async Task<ActionResult<List<LightProductObject>>>GetProductsUnderMinQuantity(int pageNumber)
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

        [HttpGet("get-by-name-barcode{pageNumber}/{nameOrBarcode}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<LightProductObject>>> GetProductsByNameOrBarcode(int pageNumber,string nameOrBarcode)
        {
            if (pageNumber < 1)
            {
                return BadRequest($"Not Accepted PageNumber !{pageNumber}");
            }
            if(string.IsNullOrWhiteSpace(nameOrBarcode))
            {
                return BadRequest("Name Or Barcode Can Not Be Null!");
            }
            var products = await _service.GetProductByNameOrBarcode(pageNumber,nameOrBarcode);
            if (products == null || !products.Any())
            {
                return NotFound("No Products Under The Min Quantity Found !");
            }
            return Ok(products);
        }

        [HttpGet("get-nearing-expiry-products{pageNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<LightProductObject>>> GetNearingExpiryProduct(int pageNumber)
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
         public async Task<ActionResult<DetailedProductObject>>UpdateProduct(int Id,AddUpdateProductDTO updatedProduct)
        {
            if(Id < 1)
            {
                return BadRequest($"Not Accepted Id {Id}");
            }
            var productResponse = await _service.UpdateProduct(Id, updatedProduct);
            if(!productResponse.IsSuccess)
            {
                if(productResponse.ErrorType == BusinessLayer.Enums.EnErrorTypes.NotFound)
                {
                    return NotFound(new
                    {
                        Message = productResponse.ErrorType.ToString(),
                        Error = $"No Product Found With Id = {Id}"
                    });
                }
                else if(productResponse.ErrorType == BusinessLayer.Enums.EnErrorTypes.InvalidData)
                {
                    return BadRequest(new
                    {
                        Message = productResponse.ErrorType.ToString(),
                        Errors = productResponse.Errors
                    });
                }
            }
             var productDTO = productResponse.Data;
            return Ok(productDTO);
        }
    }
}
