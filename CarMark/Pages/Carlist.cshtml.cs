using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Supabase.Postgrest.Constants;

namespace CarMark.Pages
{
    public class CarListaModel : PageModel
    {
        private readonly Supabase.Client _supabase;

        public CarListaModel(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        public List<Car> Cars { get; set; } = new();
        public List<string> Companies { get; set; } = new();
        public List<string> CarModels { get; set; } = new();

        [BindProperty(SupportsGet = true)] public string? FilterCompany { get; set; }
        [BindProperty(SupportsGet = true)] public string? FilterModel { get; set; }
        [BindProperty(SupportsGet = true)] public long? MinPrice { get; set; }
        [BindProperty(SupportsGet = true)] public long? MaxPrice { get; set; }
        [BindProperty(SupportsGet = true)] public string? FilterUsed { get; set; }

        public string? ErrorMessage { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                var allCars = await _supabase
                    .From<Car>()
                    .Select("Company, Model")
                    .Get();

                Companies = allCars.Models
                    .Select(c => c.Company)
                    .Where(c => !string.IsNullOrEmpty(c))
                    .Distinct()
                    .OrderBy(c => c)
                    .ToList();

                CarModels = allCars.Models
                    .Select(c => c.Model)
                    .Where(m => !string.IsNullOrEmpty(m))
                    .Distinct()
                    .OrderBy(m => m)
                    .ToList();

                var query = _supabase.From<Car>().Select("*");

                if (!string.IsNullOrEmpty(FilterCompany))
                    query = query.Filter("Company", Operator.Equals, FilterCompany);

                if (!string.IsNullOrEmpty(FilterModel))
                    query = query.Filter("Model", Operator.ILike, $"%{FilterModel}%");

                if (MinPrice.HasValue)
                    query = query.Filter("Price", Operator.GreaterThanOrEqual, MinPrice.Value.ToString());

                if (MaxPrice.HasValue)

                if (!string.IsNullOrEmpty(FilterUsed))
                {
                    bool usedBool = FilterUsed == "true";
                    query = query.Filter("Used", Operator.Equals, usedBool.ToString().ToLower());
                }

                query = query.Order("Price", Ordering.Ascending);

                var result = await query.Get();
                Cars = result.Models;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erro ao carregar dados: {ex.Message}";
            }
        }
    }
}
