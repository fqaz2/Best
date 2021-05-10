using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Best.Areas.Identity.Data;
using Best.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Best.Areas.Identity.Pages.Account.Manage
{
    public class DeletePersonalDataModel : PageModel
    {
        private readonly UserManager<BestUser> _userManager;
        private readonly SignInManager<BestUser> _signInManager;
        private readonly ILogger<DeletePersonalDataModel> _logger;
        private readonly IBestUsers _bestUser;

        public DeletePersonalDataModel(
            UserManager<BestUser> userManager,
            SignInManager<BestUser> signInManager,
            ILogger<DeletePersonalDataModel> logger,
            IBestUsers bestUser)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _bestUser = bestUser;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public bool RequirePassword { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = _bestUser.GetUserById(_userManager.GetUserId(User));//await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            if (RequirePassword)
            {
                if (!await _userManager.CheckPasswordAsync(user, Input.Password))
                {
                    ModelState.AddModelError(string.Empty, "Incorrect password.");
                    return Page();
                }
            }

            var result = await _bestUser.Delete(user);
            if (result == 0)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleting user with ID '{user.Id}'.");
            }

            await _signInManager.SignOutAsync();

            _logger.LogInformation("User with ID '{UserId}' deleted themselves.", user.Id);

            return Redirect("~/");
        }
    }
}
