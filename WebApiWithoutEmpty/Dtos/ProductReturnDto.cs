namespace WebApiWithoutEmpty.Dtos
{
    public class ProductReturnDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime DateAdded { get; set; }
        public int CategoryId { get; set; }
    }
}
