using CarMark.Data;
using CarMark.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CarMark.Pages;

public class CarDetailsModel(CarMarkDbContext database) : PageModel
{
    public string? ErrorMessage { get; set; }
    public Car? Car { get; set; }
    public List<CarImage> Images { get; set; } = new();
    public User? Seller { get; set; }
    public bool CanManageCar { get; private set; }

    public async Task<IActionResult> OnGetAsync(long id)
    {
        Car = await database.Cars.Include(car => car.Seller).Include(car => car.Images).AsNoTracking().SingleOrDefaultAsync(car => car.Id == id);
        if (Car is null) return NotFound();
        Images = Car.Images.Where(image => !string.IsNullOrWhiteSpace(image.ImgUrl)).ToList();
        if (!Images.Any() && !string.IsNullOrWhiteSpace(Car.Image)) Images.Add(new CarImage { CarId = Car.Id, ImgUrl = Car.Image });
        Seller = Car.Seller;
        CanManageCar = User.Identity?.IsAuthenticated == true
            && long.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var currentUserId)
            && Car.SellerId == currentUserId;
        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync(long id)
    {
        if (!User.Identity?.IsAuthenticated ?? true)
            return Challenge();

        if (!long.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var currentUserId))
            return Forbid();

        var car = await database.Cars.SingleOrDefaultAsync(item => item.Id == id);
        if (car is null)
            return NotFound();

        if (car.SellerId != currentUserId)
            return Forbid();

        database.Cars.Remove(car);
        await database.SaveChangesAsync();
        return RedirectToPage("/Carlist");
    }
}
