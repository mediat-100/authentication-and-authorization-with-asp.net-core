using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Data.Account;

namespace WebApp_UnderTheHood.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        [BindProperty]
        public string Message { get; set; }

        private readonly UserManager<User> userManager;

        public ConfirmEmailModel(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<IActionResult> OnGetAsync(string userId, string token)
        {
            var user = await this.userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var result = await this.userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    this.Message = "Email has been successfully confirmed, Procced to Login";
                    return Page();
                }
            }

            this.Message = "Email Confirmation Failed.";
            return Page();
        }
    }
}
