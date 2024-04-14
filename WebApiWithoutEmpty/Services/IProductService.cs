using WebApiWithoutEmpty.Dtos;

namespace WebApiWithoutEmpty.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
        Task<ProductDto> GetProductById(int id);
        ProductDto CreateProduct(ProductReturnDto productdto, int userid);
        void UpdateProduct(ProductDto product);
        void DeleteProduct(int id);
    }
}
