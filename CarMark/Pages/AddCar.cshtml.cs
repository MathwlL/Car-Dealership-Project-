using CarMark.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Supabase.Postgrest.Constants;

namespace CarMark.Pages
{
    public class AddCarModel : PageModel
    {
        private const long MaxImageBytes = 5 * 1024 * 1024;
        private static readonly HashSet<string> AllowedImageTypes = new(StringComparer.OrdinalIgnoreCase)
        {
            "image/jpeg",
            "image/png",
            "image/webp",
            "image/gif"
        };

        private readonly Supabase.Client _supabase;

        public AddCarModel(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        [BindProperty]
        public CarFormInput Input { get; set; } = new();

        [BindProperty]
        public List<IFormFile> Images { get; set; } = new();

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
                var seller = await FindSellerByEmailAsync(Input.SellerEmail);

                if (seller == null)
                {
                    ErrorMessage = "Register as a seller before adding a car, then use the same seller email here.";
                    return Page();
                }

                var imageHashes = await ReadImagesAsDataUrlsAsync();

                var car = new Car
                {
                    Company = Input.Company.Trim(),
                    Model = Input.Model.Trim(),
                    Price = Input.Price,
                    Year = Input.Year,
                    Used = Input.Used,
                    Details = Input.Details?.Trim(),
                    Image = imageHashes.FirstOrDefault(),
                    SellerId = seller.Id
                };

                var createdCar = await _supabase.From<Car>().Insert(car);
                var savedCar = createdCar.Models.FirstOrDefault();

                if (savedCar == null || savedCar.Id <= 0)
                {
                    ErrorMessage = "The car was saved, but its ID could not be read for image storage.";
                    return Page();
                }

                foreach (var imageHash in imageHashes)
                {
                    await _supabase.From<CarImage>().Insert(new CarImage
                    {
                        CarId = savedCar.Id,
                        ImgUrl = imageHash
                    });
                }

                SuccessMessage = "Car added successfully.";
                return RedirectToPage("/CarDetails", new { id = savedCar.Id });
            }
            catch (InvalidOperationException ex)
            {
                ErrorMessage = ex.Message;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error adding car: {ex.Message}";
            }

            return Page();
        }

        private async Task<List<string>> ReadImagesAsDataUrlsAsync()
        {
            var imageHashes = new List<string>();

            foreach (var image in Images.Where(image => image.Length > 0))
            {
                if (!AllowedImageTypes.Contains(image.ContentType))
                    throw new InvalidOperationException("Use only JPEG, PNG, WebP, or GIF images.");

                if (image.Length > MaxImageBytes)
                    throw new InvalidOperationException("Each image must be 5 MB or smaller.");

                await using var stream = image.OpenReadStream();
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);

                var base64 = Convert.ToBase64String(memoryStream.ToArray());
                imageHashes.Add($"data:{image.ContentType};base64,{base64}");
            }

            return imageHashes;
        }

        private async Task<Users?> FindSellerByEmailAsync(string email)
        {
            var result = await _supabase
                .From<Users>()
                .Filter("email", Operator.Equals, email.Trim())
                .Get();

            return result.Models.FirstOrDefault(user => user.IsSeller);
        }

        public class CarFormInput
        {
            [Required]
            [EmailAddress]
            [StringLength(160)]
            public string SellerEmail { get; set; } = string.Empty;

            [Required]
            [StringLength(80)]
            public string Company { get; set; } = string.Empty;

            [Required]
            [StringLength(100)]
            public string Model { get; set; } = string.Empty;

            [Range(0, long.MaxValue)]
            public long Price { get; set; }

            [Range(1900, 2100)]
            public int Year { get; set; } = DateTime.UtcNow.Year;

            public bool Used { get; set; }

            [StringLength(3000)]
            public string? Details { get; set; }
        }
    }
}
