using CarMark.Data;
using CarMark.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CarMark.Pages;

public class CarListaModel(CarMarkDbContext database) : PageModel
{
    public string? ErrorMessage { get; set; }
    public List<Car> Cars { get; set; } = new();
    public List<string> Companies { get; set; } = new();
    [BindProperty(SupportsGet = true)] public string? FilterCompany { get; set; }
    [BindProperty(SupportsGet = true)] public string? FilterModel { get; set; }
    [BindProperty(SupportsGet = true)] public long? MinPrice { get; set; }
    [BindProperty(SupportsGet = true)] public long? MaxPrice { get; set; }
    [BindProperty(SupportsGet = true)] public string? FilterUsed { get; set; }

    public async Task OnGetAsync()
    {
        Companies = await database.Cars.Select(car => car.Company).Distinct().OrderBy(company => company).ToListAsync();
        var query = database.Cars.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(FilterCompany)) query = query.Where(car => car.Company == FilterCompany);
        if (!string.IsNullOrWhiteSpace(FilterModel)) query = query.Where(car => car.Model.Contains(FilterModel));
        if (MinPrice.HasValue) query = query.Where(car => car.Price >= MinPrice.Value);
        if (MaxPrice.HasValue) query = query.Where(car => car.Price <= MaxPrice.Value);
        if (FilterUsed is "true" or "false") query = query.Where(car => car.Used == (FilterUsed == "true"));
        Cars = await query.OrderBy(car => car.Price).ToListAsync();
    }
}
