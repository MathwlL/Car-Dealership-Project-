using CarMark.Data;
using CarMark.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace CarMark.Pages;

[Authorize]
public class ProfileModel(CarMarkDbContext database) : PageModel
{
    private const long MaxImageBytes = 2 * 1024 * 1024;
    private static readonly HashSet<string> AllowedImageTypes = new(StringComparer.OrdinalIgnoreCase) { "image/jpeg", "image/png", "image/webp" };
    [BindProperty] public ProfileInput Input { get; set; } = new();
    [BindProperty] public IFormFile? ProfilePhoto { get; set; }
    public string? ProfileImage { get; set; }
    public string? SuccessMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await CurrentUserAsync();
        if (user is null) return Challenge();
        Input = new ProfileInput { Name = user.Name, Phone = user.Phone, BirthDate = user.BirthDate, ShowProfileImage = user.ShowProfileImage, ShowEmail = user.ShowEmail, ShowPhone = user.ShowPhone, ShowBirthDate = user.ShowBirthDate };
        ProfileImage = user.ProfileImage;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await CurrentUserAsync();
        if (user is null) return Challenge();
        if (!ModelState.IsValid) { ProfileImage = user.ProfileImage; return Page(); }

        user.Name = Input.Name.Trim();
        user.Phone = Input.Phone.Trim();
        user.BirthDate = Input.BirthDate;
        user.ShowProfileImage = Input.ShowProfileImage;
        user.ShowEmail = Input.ShowEmail;
        user.ShowPhone = Input.ShowPhone;
        user.ShowBirthDate = Input.ShowBirthDate;
        if (ProfilePhoto is { Length: > 0 }) user.ProfileImage = await ReadImageAsync(ProfilePhoto);
        await database.SaveChangesAsync();
        await SignInAsync(user);
        ProfileImage = user.ProfileImage;
        SuccessMessage = "Profile updated.";
        return Page();
    }

    private async Task<User?> CurrentUserAsync()
    {
        if (!long.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id)) return null;
        return await database.Users.SingleOrDefaultAsync(item => item.Id == id);
    }

    private static async Task<string> ReadImageAsync(IFormFile image)
    {
        if (!AllowedImageTypes.Contains(image.ContentType)) throw new InvalidOperationException("Use a JPEG, PNG, or WebP image.");
        if (image.Length > MaxImageBytes) throw new InvalidOperationException("Profile photo must be 2 MB or smaller.");
        await using var stream = image.OpenReadStream(); using var memory = new MemoryStream();
        await stream.CopyToAsync(memory);
        return $"data:{image.ContentType};base64,{Convert.ToBase64String(memory.ToArray())}";
    }

    private Task SignInAsync(User user) => HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
        new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), new Claim(ClaimTypes.Name, user.Name), new Claim(ClaimTypes.Email, user.Email), new Claim(ClaimTypes.Role, user.IsSeller ? "Seller" : "Buyer") }, CookieAuthenticationDefaults.AuthenticationScheme)));

    public class ProfileInput
    {
        [Required, StringLength(100)] public string Name { get; set; } = string.Empty;
        [Required, Phone, StringLength(30)] public string Phone { get; set; } = string.Empty;
        [DataType(DataType.Date)] public DateTime? BirthDate { get; set; }
        public bool ShowProfileImage { get; set; }
        public bool ShowEmail { get; set; }
        public bool ShowPhone { get; set; }
        public bool ShowBirthDate { get; set; }
    }
}
