using System.ComponentModel.DataAnnotations;

namespace UserManagement.Models.Security
{
    public class LoginViewModel
    {
        //Required attribute'ü ile kullanıcının giriş yapmasını zorunlu kıldık.
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
