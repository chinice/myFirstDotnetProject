using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyLearning.Dtos;
using MyLearning.Models;
using MyLearning.Services;

namespace MyLearning.Data
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("AllowOrigin")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns>Products retrieved successfully</returns>
        /// <returns code="200">Products retrieved successfully</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseMessage))]
        public async Task<IActionResult> GetProduct()
        {
            var products = await _productRepository.GetAllProducts();

            return Ok(new
            {
                Status = true,
                Message = "Products retrieved successfully",
                Data = products
            });
        }

        /// <summary>
        /// Get specific product by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Product retrieved successfully</returns>
        /// <returns code="200">Product retrieved successfully</returns>
        /// <returns code="422">The product was not found</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productRepository.GetProductById(id);

            if (product == null)
            {
                return StatusCode(422, new
                {
                    Status = false,
                    Message = "The product was not found",
                    Data = new { }
                });
            }

            return Ok(new
            {
                Status = true,
                Message = "Product retrieved successfully",
                Data = product
            });
        }

        /// <summary>
        /// Update a product
        /// </summary>
        /// <param name="productCreateDto"></param>
        /// <param name="id"></param>
        /// <returns>Product was successfully updated</returns>
        /// <returns code="201">Product was successfully updated</returns>
        /// <returns code="422">The product was not found</returns>
        /// <returns code="500">Internal Server Error</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseMessage))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ResponseMessage))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseMessage))]
        public async Task<IActionResult> PutProduct([FromBody] ProductCreateDto productCreateDto, int id)
        {
            var product = await _productRepository.GetProductById(id);

            if (product == null)
            {
                return StatusCode(422, new
                {
                    Status = false,
                    Message = "The product was not found",
                    Data = new { }
                });
            }

            product.ProductName = productCreateDto.ProductName;
            product.ProductDescription = productCreateDto.ProductDescription;
            product.ProductAmount = productCreateDto.ProductAmount;
            product.Quantity = productCreateDto.Quantity;

            var result = await _productRepository.UpdateProduct(product);
            if (result)
            {
                return StatusCode(201, new
                {
                    Status = true,
                    Message = "Product successfully updated",
                    Data = product
                });
            }
            else
            {
                return StatusCode(500, new
                {
                    Status = false,
                    Message = "Internal server error",
                    Data = new { }
                });
            }

        }


        /// <summary>
        /// Create a new product
        /// </summary>
        /// <param name="productCreateDto"></param>
        /// <returns>Product successfully created</returns>
        /// <returns code="201">Product successfully created</returns>
        /// <returns code="400">Model state error</returns>
        /// <returns code="500">Internal Server Error</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseMessage))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseMessage))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseMessage))]
        public async Task<IActionResult> PostProduct([FromBody] ProductCreateDto productCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Status = false,
                    Message = ModelState,
                    Data = new { }
                });
            }

            var product = await _productRepository.AddProduct(new Models.Product
            {
                ProductName = productCreateDto.ProductName,
                ProductDescription = productCreateDto.ProductDescription,
                ProductAmount = productCreateDto.ProductAmount,
                Quantity = productCreateDto.Quantity,
                CreatedAt = DateTime.Now
            });

            if (product.Id > 0)
            {
                return StatusCode(201, new
                {
                    Status = true,
                    Message = "Product successfully created",
                    Data = product
                });
            }
            else
            {
                return StatusCode(500, new
                {
                    Status = false,
                    Message = "Internal server error",
                    Data = new { }
                });
            }
        }

        /// <summary>
        /// Delete a product
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Product was successfully deleted</returns>
        /// <returns code="200">Product was successfully deleted</returns>
        /// <returns code="422">Product not found</returns>
        /// <returns code="500">Internal Server Error</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseMessage))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ResponseMessage))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseMessage))]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productRepository.GetProductById(id);
            if(product == null)
            {
                return StatusCode(422, new
                {
                    Status = false,
                    Message = "Product not found",
                    Data = new { }
                });
            }

            var result = await _productRepository.DeleteProduct(product);

            if (result)
            {
                return StatusCode(200, new
                {
                    Status = true,
                    Message = "Product successfully deleted",
                    Data = new { }
                });
            }
            else
            {
                return StatusCode(500, new
                {
                    Status = false,
                    Message = "Internal server error",
                    Data = new { }
                });
            }
        }
    }
}
