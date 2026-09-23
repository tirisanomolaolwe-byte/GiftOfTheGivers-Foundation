using System.ComponentModel.DataAnnotations;
using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiftOfTheGivers.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        [TempData]
        public string? ErrorMessage { get; set; }

        public string? ReturnUrl { get; set; }

        // Role passed from the login link, e.g. "Donor" or "Employee"
        [BindProperty(SupportsGet = true)]
        public string? Role { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
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

            _logger.LogInformation(
                "Login attempt for {Email}. Role intent: {Role}",
                Input.Email,
                Role);

            // Explicitly use ASP.NET Core Identity SignInResult
            Microsoft.AspNetCore.Identity.SignInResult result;

            try
            {
                result = await _signInManager.PasswordSignInAsync(
                    Input.Email,
                    Input.Password,
                    Input.RememberMe,
                    lockoutOnFailure: false);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Exception during login attempt for {Email}",
                    Input.Email);

                ModelState.AddModelError(
                    string.Empty,
                    "An unexpected error occurred while signing in. Please try again later.");

                return Page();
            }

            if (result.Succeeded)
            {
                _logger.LogInformation(
                    "User {Email} successfully logged in.",
                    Input.Email);

                var user = await _userManager.FindByEmailAsync(Input.Email);

                if (user != null && !string.IsNullOrEmpty(Role))
                {
                    var isInRole = await _userManager.IsInRoleAsync(user, Role);

                    // Employee login protection
                    if (!isInRole &&
                        Role.Equals(
                            "Employee",
                            StringComparison.OrdinalIgnoreCase))
                    {
                        await _signInManager.SignOutAsync();

                        _logger.LogWarning(
                            "User {Email} attempted employee sign-in without Employee role.",
                            Input.Email);

                        ModelState.AddModelError(
                            string.Empty,
                            "You do not have employee access.");

                        return Page();
                    }

                    // Employee dashboard
                    if (Role.Equals(
                            "Employee",
                            StringComparison.OrdinalIgnoreCase) &&
                        isInRole)
                    {
                        _logger.LogInformation(
                            "Redirecting employee {Email} to Employee dashboard.",
                            Input.Email);

                        return LocalRedirect("~/Employee/Index");
                    }
                }

                // Default successful login
                _logger.LogInformation(
                    "Redirecting user {Email} to {ReturnUrl}.",
                    Input.Email,
                    returnUrl);

                return LocalRedirect(returnUrl);
            }

            // Account locked
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account is locked out.");

                return RedirectToPage("/Account/Lockout");
            }

            // Two-factor authentication
            if (result.RequiresTwoFactor)
            {
                _logger.LogInformation(
                    "Two-factor authentication required for {Email}.",
                    Input.Email);

                return RedirectToPage(
                    "/Account/LoginWith2fa",
                    new
                    {
                        ReturnUrl = returnUrl,
                        RememberMe = Input.RememberMe
                    });
            }

            // Account not allowed to sign in
            if (result.IsNotAllowed)
            {
                _logger.LogWarning(
                    "Sign-in not allowed for {Email}.",
                    Input.Email);

                ModelState.AddModelError(
                    string.Empty,
                    "Sign-in is not allowed for this account.");

                return Page();
            }

            // Invalid credentials
            ModelState.AddModelError(
                string.Empty,
                "Invalid login attempt.");

            return Page();
        }
    }
}