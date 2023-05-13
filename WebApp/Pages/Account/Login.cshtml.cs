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

        public LoginModel(SignInManager<User> signInManager)
        {
            this.signInManager = signInManager;
        }

        [BindProperty]
        public CredentialViewModel CredentialViewModel { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

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
