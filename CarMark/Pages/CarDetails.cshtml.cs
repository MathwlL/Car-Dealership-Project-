using CarMark.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Supabase.Postgrest.Constants;

namespace CarMark.Pages
{
    public class CarDetailsModel : PageModel
    {
        private readonly Supabase.Client _supabase;

        public CarDetailsModel(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        public Car? Car { get; set; }
        public List<CarImage> Images { get; set; } = new();
        public Users? Seller { get; set; }
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            try
            {
                var carResult = await _supabase
                    .From<Car>()
                    .Filter("id", Operator.Equals, id.ToString())
                    .Single();

                if (carResult == null)
                    return NotFound();

                Car = carResult;

                var imagesResult = await _supabase
                    .From<CarImage>()
                    .Select("*")
                    .Filter("CarID", Operator.Equals, id.ToString())
                    .Get();

                Images = imagesResult.Models
                    .Where(image => !string.IsNullOrWhiteSpace(image.ImgUrl))
                    .ToList();

                if (!Images.Any() && !string.IsNullOrWhiteSpace(Car.Image))
                {
                    Images.Add(new CarImage
                    {
                        CarId = Car.Id,
                        ImgUrl = Car.Image
                    });
                }

                if (Car.SellerId.HasValue)
                {
                    var sellerResult = await _supabase
                        .From<Users>()
                        .Filter("id", Operator.Equals, Car.SellerId.Value.ToString())
                        .Get();

                    Seller = sellerResult.Models.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading car: {ex.Message}";
            }

            return Page();
        }
    }
}
