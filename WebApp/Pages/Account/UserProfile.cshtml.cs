using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Data.Account;

namespace WebApp.Pages.Account
{
    [Authorize]
    public class UserProfileModel : PageModel
    {
        private readonly UserManager<User> userManager;

        [BindProperty]
        public UserProfileViewModel UserProfile { get; set; }

        public UserProfileModel(UserManager<User> userManager)
        {
            this.userManager = userManager;
            this.UserProfile = new UserProfileViewModel();
        }
        public  async Task<IActionResult> OnGetAsync()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var claims = await userManager.GetClaimsAsync(user);
            var departmentClaim = claims.FirstOrDefault(x => x.Type == "Department");
            var positionClaim = claims.FirstOrDefault(x => x.Type == "Position");

            this.UserProfile.Email = User.Identity.Name;
            this.UserProfile.Department = departmentClaim?.Value;
            this.UserProfile.Position = positionClaim?.Value;

            return Page();
        }
    }

    public class UserProfileViewModel
    {
        public string Email { get; set; }

        [Required]
        public string Department { get; set; }
        
        [Required]
        public string Position { get; set; }
    }
}
