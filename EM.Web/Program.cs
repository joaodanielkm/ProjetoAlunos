using EM.Repository;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<Contexto>(x => x.UseFirebird(@"database=C:\Users\joaod\source\repos\ProjetoEM\EM.Repository\Banco\DBPROJETOEM.FD4;DataSource=localhost;Dialect=3;Charset=NONE;Pooling=true;user=sysdba;password=masterkey;dialect=3"));
//builder.Services.AddDbContext<Contexto>(x => x.UseFirebird(@"database=192.168.0.168/3054:C:\Users\joaod\source\repos\ProjetoEM\EM.Repository\Banco\DBPROJETOEM.FD4;DataSource=localhost;Dialect=3;Charset=NONE;Pooling=true;user=sysdba;password=masterkey;dialect=3"));
builder.Services.AddTransient<IAlunoRepository, AlunoRepository>();

builder.Services.AddControllersWithViews()
                    .AddRazorRuntimeCompilation()
                    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                    .AddDataAnnotationsLocalization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
