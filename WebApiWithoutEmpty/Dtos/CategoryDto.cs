using WebApiWithoutEmpty.Entities;

namespace WebApiWithoutEmpty.Dtos
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Product> Products {  get; set; } 
    }
}
