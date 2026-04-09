using Microsoft.AspNetCore.Mvc.RazorPages;
using Supabase;
using CarMark;

public class CarlistModel : PageModel
{
    private readonly Client _supabase;

    public List<Carro> Carros { get; set; } = new();

    public CarlistModel(Client supabase)
    {
        _supabase = supabase;
    }

    public async Task OnGetAsync()
    {
        var response = await _supabase
            .From<Carro>()
            .Get();

        Carros = response.Models;
    }
}