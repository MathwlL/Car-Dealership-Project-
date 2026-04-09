using Supabase;

var builder = WebApplication.CreateBuilder(args);

//Supabase
var supabaseUrl = "https://qfucawohiyffmjmtldhj.supabase.co";
var supabaseKey = "sb_publishable_sD04tphf_dP3LKw02xwrQg_SoG049gM";

var supabase = new Client(supabaseUrl, supabaseKey);
await supabase.InitializeAsync();

//adicionar ao sistema de dependência
builder.Services.AddSingleton(supabase);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
