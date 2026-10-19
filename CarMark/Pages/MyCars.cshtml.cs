using CarMark.Data;
using CarMark.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CarMark.Pages;

[Authorize]
public class MyCarsModel(CarMarkDbContext database) : PageModel
{
    public List<Car> Cars { get; set; } = new();
    public async Task OnGetAsync()
    {
        if (!long.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId)) return;
        Cars = await database.Cars.AsNoTracking().Where(car => car.SellerId == userId).OrderByDescending(car => car.CreatedAt).ToListAsync();
    }
}
