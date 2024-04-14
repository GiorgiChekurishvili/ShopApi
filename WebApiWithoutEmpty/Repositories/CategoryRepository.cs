using Microsoft.EntityFrameworkCore;
using WebApiWithoutEmpty.Entities;

namespace WebApiWithoutEmpty.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ShopContext _shopContext;

        public CategoryRepository(ShopContext shopContext)
        {
            _shopContext = shopContext;
        }
        

        public IEnumerable<Category> Getall()
        {
            var category = _shopContext.Categories.ToList();

            return category;
        }

        public Category Getbyid(int id)
        {
            var categorybyid = _shopContext.Categories.Find(id);
            return categorybyid;
        }

        public Category Insert(Category category)
        {
            _shopContext.Categories.Add(category);
            _shopContext.SaveChanges();
            return category;
        }

        public void Update(Category category)
        {
            _shopContext.Entry(category).State = EntityState.Modified;
            _shopContext.SaveChanges();
        }
        public void Delete(Category category)
        {
            _shopContext.Categories.Remove(category);
            _shopContext.SaveChanges();
        }
    }
}
