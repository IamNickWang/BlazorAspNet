//using BlazorCrud.Data;
//using BlazorCrud.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.EntityFrameworkCore;
using TransferOrderRolling.Components;
using TransferOrderRolling.Model;
using TransferOrderRolling.Service;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddDbContext<DatabaseContext>(options => 
        options.UseSqlServer(builder.Configuration.GetConnectionString("conn")));
builder.Services.AddTransient<ITransferOrder, TransferOrderService>();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.MapRazorPages();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
