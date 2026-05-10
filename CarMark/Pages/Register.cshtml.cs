using CarMark.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Supabase.Postgrest.Constants;

namespace CarMark.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly Supabase.Client _supabase;

        public RegisterModel(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        [BindProperty]
        public RegisterInput Input { get; set; } = new();

        public string? ErrorMessage { get; set; }
        public string? SuccessMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                var existingUser = await _supabase
                    .From<Users>()
                    .Filter("email", Operator.Equals, Input.Email.Trim())
                    .Get();

                if (existingUser.Models.Any())
                {
                    ErrorMessage = "A user with this email already exists.";
                    return Page();
                }

                var user = new Users
                {
                    Name = Input.Name.Trim(),
                    Email = Input.Email.Trim(),
                    Phone = Input.Phone.Trim(),
                    IsSeller = Input.AccountType is "seller" or "both",
                    IsBuyer = Input.AccountType is "buyer" or "both"
                };

                await _supabase.From<Users>().Insert(user);

                SuccessMessage = "Registration complete. Sellers can now add cars using this email.";
                Input = new RegisterInput();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error registering user: {ex.Message}";
            }

            return Page();
        }

        public class RegisterInput
        {
            [Required]
            [StringLength(100)]
            public string Name { get; set; } = string.Empty;

            [Required]
            [EmailAddress]
            [StringLength(160)]
            public string Email { get; set; } = string.Empty;

            [Required]
            [Phone]
            [StringLength(30)]
            public string Phone { get; set; } = string.Empty;

            [Required]
            public string AccountType { get; set; } = "buyer";
        }
    }
}
