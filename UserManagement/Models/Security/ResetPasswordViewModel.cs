using System.ComponentModel.DataAnnotations;

namespace UserManagement.Models.Security
{
    public class ResetPasswordViewModel
    {
        //Required attribute'ü ile kullanıcının şifre yenilemesi yaparken istenilen tüm verileri zorunlu kıldık.
        [Required]
        public string Code { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
