using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using WebApp.Data.Account;

namespace WebApp.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;
        private readonly UserManager<User> userManager;

        public PrivacyModel(ILogger<PrivacyModel> logger, UserManager<User> userManager)
        {
            _logger = logger;
            this.userManager = userManager;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await this.userManager.FindByNameAsync(User.Identity.Name);

            if (user.TwoFactorEnabled)
            {
                ModelState.AddModelError("2fa","2fa enabled already");
                return Page();
            }
            await this.userManager.SetTwoFactorEnabledAsync(user, true);

            return RedirectToPage("/TwoFactorMessage");
        }
    }
}
