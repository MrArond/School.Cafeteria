using School.Cafeteria.Domain.Abstract;
using School.Cafeteria.Domain.Entities;

namespace School.Cafeteria.Domain.Concrete
{
    public class EFProductRepository : IEFProductRepository
    {
        private readonly EFDbContext _context;
        public EFProductRepository(EFDbContext dbContext)
        {
            this._context = dbContext;
        }

        public Product CreateProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();

            return product;
        }

        public void RemoveProduct(int productId)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == productId);

            if (product == null)
            {
                return;
            }

            _context.Products.Remove(product);
            _context.SaveChanges();
        }

        public Product GetProductById(int productId)
        {
            return _context.Products.FirstOrDefault(p => p.ProductId == productId);
        }

        public IEnumerable<Product> GetProducts()
        {
            return this._context.Products;
        }

        public Product UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();

            return product;
        }
    }
}
