using System.Net;

using Microsoft.AspNetCore.Mvc;

using School.Cafeteria.Domain.Entities;
using School.Cafeteria.Models.Models.Product;
using School.Cafeteria.Services.Interfaces;

using Swashbuckle.AspNetCore.Annotations;

namespace School.Cafeteria.Controllers
{
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            this._productService = productService;
        }

        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns>List of products.</returns>
        [HttpGet]
        [Route("products")]
        [SwaggerOperation(
            Summary = "Gets products",
            Description = "Gets products",
            Tags = new[] { "Product" })]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(IEnumerable<Product>))]
        [SwaggerResponse((int)HttpStatusCode.NoContent)]
        public IActionResult GetProducts()
        {
            var products = this._productService.GetProducts();

            if (!products.Any())
            {
                return NoContent();
            }

            return this.Ok(products);
        }

        /// <summary>
        /// Creates products.
        /// </summary>
        /// <returns>New product.</returns>
        [HttpPost]
        [Route("product/create")]
        [SwaggerOperation(
            Summary = "Creates products",
            Description = "Creates products",
            Tags = new[] { "Product" })]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Product))]
        public IActionResult CreateProduct([FromBody] ProductDto product)
        {
            var newProduct = this._productService.CreateProduct(product);

            return this.Ok(newProduct);
        }

        /// <summary>
        /// Updates products.
        /// </summary>
        /// <returns>List of produts</returns>
        [HttpPut]
        [Route("product/update")]
        [SwaggerOperation(
            Summary = "Updates products",
            Description = "Updates products",
            Tags = new[] { "Product" })]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Product))]
        public IActionResult UpdateProduct([FromBody] ProductDto product)
        {
            var updatedProduct = this._productService.UpdateProduct(product);

            return this.Ok(updatedProduct);
        }

        /// <summary>
        /// Removes products.
        /// </summary>
        [HttpDelete]
        [Route("product/{productId}/remove")]
        [SwaggerOperation(
            Summary = "Removes products",
            Description = "Removes products",
            Tags = new[] { "Product" })]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        public IActionResult RemoveProduct([FromRoute] int productId)
        {
            this._productService.RemoveProduct(productId);

            return this.Ok();
        }
    }
}
