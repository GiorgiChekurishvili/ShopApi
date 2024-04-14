namespace WebApiWithoutEmpty.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;

        public ICollection<UserRole> userRoles { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<Category> Categories { get; set; }
    }
}
