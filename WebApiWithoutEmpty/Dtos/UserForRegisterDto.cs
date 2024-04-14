using System.ComponentModel.DataAnnotations;

namespace WebApiWithoutEmpty.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public string Email { get; set; }
        [Required]
        public int RoleId { get; set; }
    }
}
