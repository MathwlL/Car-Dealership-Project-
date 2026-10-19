using CarMark.Data;
using CarMark.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CarMark.Pages;

public class SellerProfileModel(CarMarkDbContext database) : PageModel
{
    public User? Seller { get; set; }
    public List<Car> Cars { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(long id)
    {
        Seller = await database.Users.AsNoTracking().SingleOrDefaultAsync(user => user.Id == id && user.IsSeller);
        if (Seller is null) return NotFound();
        Cars = await database.Cars.AsNoTracking().Where(car => car.SellerId == id).OrderByDescending(car => car.CreatedAt).ToListAsync();
        return Page();
    }
}
