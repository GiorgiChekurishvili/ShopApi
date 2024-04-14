using WebApiWithoutEmpty.Entities;

namespace WebApiWithoutEmpty.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<Product> Getall();
        Product GetById(int id);
        Product Insert(Product product);
        void Update(Product product);
        void Delete(Product product);

    }
}
