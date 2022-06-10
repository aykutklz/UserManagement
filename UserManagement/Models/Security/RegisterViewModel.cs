using System.ComponentModel.DataAnnotations;

namespace UserManagement.Models.Security
{
    public class RegisterViewModel
    {
        //Required attribute'ü ile kullanıcının kayıt yaparken istenilen tüm verileri zorunlu kıldık.
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string ConfirmedPassword { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public int Age { get; set; }
    }
}
