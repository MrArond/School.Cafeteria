using School.Cafeteria.Domain.Abstract;
using School.Cafeteria.Domain.Entities;
using School.Cafeteria.Models.Models.Product;
using School.Cafeteria.Services.Interfaces;

namespace School.Cafeteria.Services.Services
{
    public class ProductService : IProductService
    {
        private readonly IEFProductRepository _productRepository;

        public ProductService(IEFProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }

        public Product CreateProduct(ProductDto product)
        {
            var newProduct = new Product
            {
                Description = product.Description,
                Image = product.Image,
                IsAvailable = product.IsAvailable,
                Name = product.Name,
                Price = product.Price.Value,
                Quantity = product.Quantity.Value,
            };

            return this._productRepository.CreateProduct(newProduct);
        }

        public void RemoveProduct(int productId)
        {
            this._productRepository.RemoveProduct(productId);
        }

        public IEnumerable<Product> GetProducts()
        {
            return this._productRepository.GetProducts() ?? Enumerable.Empty<Product>();
        }

        public Product UpdateProduct(ProductDto product)
        {
            if (product.ProductId == null)
            {
                throw new Exception("Missing product ID.");
            }

            var existingProduct = this._productRepository.GetProductById(product.ProductId.Value);

            if (existingProduct == null)
            {
                throw new Exception("Product doesn't exists.");
            }

            existingProduct.Quantity = product.Quantity.HasValue ? product.Quantity.Value : existingProduct.Quantity;
            existingProduct.Price = product.Price.HasValue ? product.Price.Value : existingProduct.Price;
            existingProduct.Name = string.IsNullOrEmpty(product.Name) ? existingProduct.Name : product.Name;
            existingProduct.Description = string.IsNullOrEmpty(product.Description) ? existingProduct.Description : product.Description;
            existingProduct.Image = string.IsNullOrEmpty(product.Image) ? existingProduct.Image : product.Image;
            existingProduct.IsAvailable = product.IsAvailable;

            return this._productRepository.UpdateProduct(existingProduct);
        }
    }
}
