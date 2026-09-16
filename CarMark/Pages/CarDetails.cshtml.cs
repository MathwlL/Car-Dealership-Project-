using CarMark.Data;
using CarMark.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CarMark.Pages;

public class CarDetailsModel(CarMarkDbContext database) : PageModel
{
    public string? ErrorMessage { get; set; }
    public Car? Car { get; set; }
    public List<CarImage> Images { get; set; } = new();
    public User? Seller { get; set; }

    public async Task<IActionResult> OnGetAsync(long id)
    {
        Car = await database.Cars.Include(car => car.Seller).Include(car => car.Images).AsNoTracking().SingleOrDefaultAsync(car => car.Id == id);
        if (Car is null) return NotFound();
        Images = Car.Images.Where(image => !string.IsNullOrWhiteSpace(image.ImgUrl)).ToList();
        if (!Images.Any() && !string.IsNullOrWhiteSpace(Car.Image)) Images.Add(new CarImage { CarId = Car.Id, ImgUrl = Car.Image });
        Seller = Car.Seller;
        return Page();
    }
}
