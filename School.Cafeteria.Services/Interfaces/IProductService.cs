using School.Cafeteria.Domain.Entities;
using School.Cafeteria.Models.Models.Product;

namespace School.Cafeteria.Services.Interfaces
{
    public interface IProductService
    {
        IEnumerable<Product> GetProducts();
        Product CreateProduct(ProductDto product);
        Product UpdateProduct(ProductDto product);
        void RemoveProduct(int productId);
    }
}
