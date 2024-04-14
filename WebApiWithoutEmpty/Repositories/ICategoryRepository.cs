using WebApiWithoutEmpty.Entities;

namespace WebApiWithoutEmpty.Repositories
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> Getall();
        Category Getbyid(int id);
        Category Insert(Category category);
        void Update(Category category);
        void Delete(Category category);
    }
}
