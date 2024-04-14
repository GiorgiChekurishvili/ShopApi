using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiWithoutEmpty.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal Price { get; set; }
        public DateTime DateAdded { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }
    }
}
