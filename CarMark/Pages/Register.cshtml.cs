using CarMark.Data;
using CarMark.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace CarMark.Pages;

public class RegisterModel(CarMarkDbContext database) : PageModel
{
    [BindProperty] public RegisterInput Input { get; set; } = new();
    public string? ErrorMessage { get; set; }
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        var email = Input.Email.Trim().ToLowerInvariant();
        if (database.Users.Any(user => user.Email == email)) { ErrorMessage = "Já existe uma conta com este e-mail."; return Page(); }
        var user = new User { Name = Input.Name.Trim(), Email = email, Phone = Input.Phone.Trim(), IsSeller = Input.AccountType is "seller" or "both", IsBuyer = Input.AccountType is "buyer" or "both" };
        user.PasswordHash = new PasswordHasher<User>().HashPassword(user, Input.Password);
        database.Users.Add(user); await database.SaveChangesAsync();
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), new Claim(ClaimTypes.Name, user.Name), new Claim(ClaimTypes.Email, user.Email), new Claim(ClaimTypes.Role, user.IsSeller ? "Seller" : "Buyer") }, CookieAuthenticationDefaults.AuthenticationScheme)));
        return RedirectToPage(user.IsSeller ? "/AddCar" : "/Carlist");
    }
    public class RegisterInput
    {
        [Required, StringLength(100)] public string Name { get; set; } = string.Empty;
        [Required, EmailAddress, StringLength(160)] public string Email { get; set; } = string.Empty;
        [Required, Phone, StringLength(30)] public string Phone { get; set; } = string.Empty;
        [Required, StringLength(100, MinimumLength = 6), DataType(DataType.Password)] public string Password { get; set; } = string.Empty;
        [Required] public string AccountType { get; set; } = "buyer";
    }
}
