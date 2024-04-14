using Microsoft.EntityFrameworkCore;
using WebApiWithoutEmpty.Entities;

namespace WebApiWithoutEmpty.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShopContext _context;


        public ProductRepository(ShopContext context)
        {
            _context = context;
        }
        public IEnumerable<Product> Getall()
        {
            var products = _context.Products.ToList();
            return products;
        }

        public Product GetById(int id)
        {
            //var productbyid = _context.Products.Where(x => x.CategoryId == id).Include(x => x.Category).FirstOrDefault();
            var productbyid = _context.Products.Find(id);
            return productbyid;
        }

        public Product Insert(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return product;
        }

        public void Update(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            _context.SaveChanges();
            
        }

        public void Delete(Product product)
        {
            
            _context.Products.Remove(product);
            _context.SaveChanges();

            
        }
    }
}
