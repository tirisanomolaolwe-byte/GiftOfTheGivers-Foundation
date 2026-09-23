using System.ComponentModel.DataAnnotations;
using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiftOfTheGivers.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        // Role intent passed from the register link ("Donor" or "Employee")
        [BindProperty(SupportsGet = true)]
        public string? Role { get; set; }

        public string? ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; } = string.Empty;
        }

        public void OnGet(string? returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email, EmailConfirmed = true };
            var result = await _userManager.CreateAsync(user, Input.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                // For security, only assign the Donor role on public registration.
                // Employee accounts must be provisioned by an administrator.
                var roleToAssign = "Donor";
                var roleAdded = await _userManager.AddToRoleAsync(user, roleToAssign);
                if (!roleAdded.Succeeded)
                {
                    _logger.LogWarning("Failed to add role {Role} to user {Email}", roleToAssign, Input.Email);
                }

                if (!string.IsNullOrEmpty(Role) && Role.Equals("Employee", StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogWarning("Registration attempted with Employee role by {Email}; self-registration for employees is not allowed.", Input.Email);
                }

                await _signInManager.SignInAsync(user, isPersistent: false);

                if (!string.IsNullOrEmpty(Role) && Role.Equals("Employee", StringComparison.OrdinalIgnoreCase))
                {
                    return LocalRedirect(Url.Content("~/Employee/Index"));
                }

                return LocalRedirect(returnUrl);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}
