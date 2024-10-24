using BlazorServerApp_AutoRefeshPage.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using BlazorServerApp_AutoRefeshPage.Model;
using BlazorServerApp_AutoRefeshPage.Service;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddDbContext<DatabaseContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("conn")));
builder.Services.AddTransient<ITransferOrder, TransferOrderService>();
builder.Services.AddSingleton<WeatherForecastService>();

//builder.Services.AddRazorComponents()
//    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();

app.UseRouting();

//app.MapRazorPages();
//app.UseStaticFiles();
//app.UseAntiforgery();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
