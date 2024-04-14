using WebApiWithoutEmpty.Dtos;

namespace WebApiWithoutEmpty.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryReturnDto>> GetCategories();
        Task<CategoryDto> GetCategoryById(int id);
        CategoryDto CreateCategory(CategoryDto category, int userid);
        void UpdateCategory(CategoryDto category);
        void DeleteCategory(int id);
    }
}
