using Microsoft.AspNetCore.Identity;

namespace UserManagement.Identity
{
    public class AppIdentityUser:IdentityUser
    {
        //IdentityUser paketi içersinde ki user property'leri haricinde property eklenebilecek sınıf
        public int Age { get; set; }    
    }
}
