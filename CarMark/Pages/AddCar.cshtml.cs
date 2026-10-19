using CarMark.Data;
using CarMark.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace CarMark.Pages;

[Authorize(Roles = "Seller")]
public class AddCarModel(CarMarkDbContext database) : PageModel
{
    private const long MaxImageBytes = 5 * 1024 * 1024;
    private static readonly HashSet<string> AllowedImageTypes = new(StringComparer.OrdinalIgnoreCase) { "image/jpeg", "image/png", "image/webp", "image/gif" };
    [BindProperty] public CarFormInput Input { get; set; } = new();
    [BindProperty] public List<IFormFile> Images { get; set; } = new();

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        var sellerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var imageData = await ReadImagesAsDataUrlsAsync();
        var car = new Car { Company = Input.Company.Trim(), Model = Input.Model.Trim(), Price = Input.Price, Currency = Input.Currency, Year = Input.Year, Used = Input.Used, Details = Input.Details?.Trim(), Image = imageData.FirstOrDefault(), SellerId = sellerId };
        database.Cars.Add(car);
        await database.SaveChangesAsync();
        database.CarImages.AddRange(imageData.Select(image => new CarImage { CarId = car.Id, ImgUrl = image }));
        await database.SaveChangesAsync();
        return RedirectToPage("/CarDetails", new { id = car.Id });
    }

    private async Task<List<string>> ReadImagesAsDataUrlsAsync()
    {
        var result = new List<string>();
        foreach (var image in Images.Where(image => image.Length > 0))
        {
            if (!AllowedImageTypes.Contains(image.ContentType)) throw new InvalidOperationException("Use apenas imagens JPEG, PNG, WebP ou GIF.");
            if (image.Length > MaxImageBytes) throw new InvalidOperationException("Cada imagem deve ter no máximo 5 MB.");
            await using var stream = image.OpenReadStream(); using var memory = new MemoryStream();
            await stream.CopyToAsync(memory);
            result.Add($"data:{image.ContentType};base64,{Convert.ToBase64String(memory.ToArray())}");
        }
        return result;
    }

    public class CarFormInput
    {
        [Required, StringLength(80)] public string Company { get; set; } = string.Empty;
        [Required, StringLength(100)] public string Model { get; set; } = string.Empty;
        [Range(0, long.MaxValue)] public long Price { get; set; }
        [Required, RegularExpression("BRL|USD|EUR|GBP")] public string Currency { get; set; } = "BRL";
        [Range(1900, 2100)] public int Year { get; set; } = DateTime.UtcNow.Year;
        public bool Used { get; set; }
        [StringLength(3000)] public string? Details { get; set; }
    }
}
