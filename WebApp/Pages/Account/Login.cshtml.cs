using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Data.Account;

namespace WebApp.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;

        public LoginModel(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        [BindProperty]
        public CredentialViewModel CredentialViewModel { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            // check if user exists and email is confirmed
            //var existingUser = await this.userManager.FindByNameAsync(this.CredentialViewModel.Email);

            /*if (!existingUser.EmailConfirmed)
            {
                ModelState.AddModelError("Login", "Please confirm your email before you sign in");
                return Page();
            }  */             

            // verify credentials
            var result = await this.signInManager.PasswordSignInAsync(this.CredentialViewModel.Email, 
                this.CredentialViewModel.Password,
                this.CredentialViewModel.RememberMe,
                false);

            if (result.Succeeded)
            {
                return Redirect("/Index");
            }
            else
            {
                if (result.RequiresTwoFactor)
                {
                    // email 2fa
                    /* return RedirectToPage(
                         "/Account/LoginTwoFactor",
                         new
                         {
                             Email = this.CredentialViewModel.Email,
                             RememberMe = this.CredentialViewModel.RememberMe
                         }
                         );*/

                    //authenticator mfa
                    return RedirectToPage("/Account/LoginTwoFactorWithAuthenticator",
                        new
                        {
                            RememberMe = this.CredentialViewModel.RememberMe
                        });
                }
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("Login", "You're locked out.");
                }
                else
                {
                    ModelState.AddModelError("Login", "Failed to login.");
                }

                return Page();
            }
        }
    }

    public class CredentialViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]   
        public string Password { get; set; }

        [Required]
        public bool RememberMe { get; set; }
    }
}
