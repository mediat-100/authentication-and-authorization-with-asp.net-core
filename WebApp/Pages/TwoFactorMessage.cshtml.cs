using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class TwoFactorMessageModel : PageModel
    {
        [BindProperty]
        public string Message { get; set; }
        public void OnGet()
        {
            this.Message = "2FA Enabled Successfully";
        }
    }
}
