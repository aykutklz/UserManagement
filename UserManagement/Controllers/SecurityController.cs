using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Identity;
using UserManagement.Models.Security;

namespace UserManagement.Controllers
{
    public class SecurityController : Controller
    {
        private UserManager<AppIdentityUser> _userManager;
        private SignInManager<AppIdentityUser> _signInManager;

        public SecurityController(UserManager<AppIdentityUser> userManager, SignInManager<AppIdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }
            var user = await _userManager.FindByNameAsync(loginViewModel.UserName);
            //Kullanıcı var mı diye veri tabanından kontrol yapıyoruz.
            if (user != null)
            {
                //kullanıcı var ise email doğrulaması yapılmış mı kontrolü
                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    ModelState.AddModelError(string.Empty, "Confirm your email please");
                    return View(loginViewModel);
                }
            }
            //Kullanıcı bilgilerinin istenilen şekilde olup olmadığının kontrolünü sağlama
            var result = await _signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password, false, false);
            //Kullanıcı doğru ise yönlendir.
            if (result.Succeeded)
            {
                //İşlemin başarılı olması durumunda boş bir sayfaya yönlendirme
                return RedirectToAction("Index", "Home1");
            }

            ModelState.AddModelError(string.Empty, "Login failed");
            return View(loginViewModel);
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            //İşlemin başarılı olması durumunda boş bir sayfaya yönlendirme
            return RedirectToAction("Index", "Home1");
        }

        //Erişim yetkisinin olmadığı bir yere gitmeye çalışırsa çalışacak kod
        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            //Model geçerli değilse baştan çalıştır
            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }

            var user = new AppIdentityUser
            {
                UserName = registerViewModel.UserName,
                Email = registerViewModel.Email,
                Age = registerViewModel.Age,
            };
            //Kullanıcı bilgilerinin istenilen şekilde olup olmadığının kontrolünü sağlama
            var result = await _userManager.CreateAsync(user, registerViewModel.Password);

            if (result.Succeeded)
            {
                //email onaylama kodu üretiyoruz.
                var confirmationCode = _userManager.GenerateEmailConfirmationTokenAsync(user);
                //Security controller'ı ve obje olarak userid ve code gönderdik.
                var callBackUrl = Url.Action("ConfirmEmail", "Security", new { userId = user.Id, code = confirmationCode.Result });
                //Send email, (Email sağlayıcısı kodları burada olmalı ve callbackurl gönderilirmeli)
                //İşlemin başarılı olması durumunda boş bir sayfaya yönlendirme
                return RedirectToAction("Index", "Home");
            }

            return View(registerViewModel);
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                //İşlemin başarısız olması durumunda boş bir sayfaya yönlendirme
                return RedirectToAction("Index", "Home1");
            }

            //veri tabanında kullanıcı var mı kontrolü
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException("Unable to find the user");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return View("ConfirmEmail");
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return View();
            }
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return View();
            }

            var confirmationCode = await _userManager.GeneratePasswordResetTokenAsync(user);

            var callBackUrl = Url.Action("ResetPassword", "Security", new { userId = user.Id, code = confirmationCode });
            //Send email, (Email sağlayıcısı kodları burada olmalı ve callbackurl gönderilirmeli)
            return RedirectToAction("ForgotPasswordEmailSent");
        }

        public IActionResult ForgotPasswordEmailSent()
        {
            return View();
        }

        public IActionResult ResetPassword(string userId, string code)
        {
            if (userId == null || code == null) { throw new ApplicationException("User Id or Code must be supplied for password reset"); }

            var model = new ResetPasswordViewModel { Code = code };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(resetPasswordViewModel);
            }
            var user = await _userManager.FindByEmailAsync(resetPasswordViewModel.Email);
            if (user == null)
            {
                throw new ApplicationException("User is not found");
            }

            //Kullanıcının şifresini değiştirmek için istenilen şekilde olup olmadığının kontrolünü sağlama
            var result = await _userManager.ResetPasswordAsync(user, resetPasswordViewModel.Code, resetPasswordViewModel.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirm");
            }
            return View();
        }

        public IActionResult ResetPasswordConfirm()
        {
            return View();
        }
    }
}
