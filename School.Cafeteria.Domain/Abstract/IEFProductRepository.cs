using School.Cafeteria.Domain.Entities;

namespace School.Cafeteria.Domain.Abstract
{
    public interface IEFProductRepository
    {
        IEnumerable<Product> GetProducts();
        Product CreateProduct(Product product);
        Product UpdateProduct(Product product);
        void RemoveProduct(int productId);
        Product GetProductById(int productId);
    }
}
