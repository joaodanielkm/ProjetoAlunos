using EM.Dominio.Interfaces;
using EM.Repository;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace EM.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IRepositorioAluno, RepositorioAluno>();
            services.AddRazorPages();
            services.AddControllersWithViews().AddRazorRuntimeCompilation().AddViewOptions(options => options.HtmlHelperOptions.ClientValidationEnabled = false);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("pt-BR"),
                SupportedCultures = new List<CultureInfo> { new("pt-BR") },
                SupportedUICultures = new List<CultureInfo> { new("pt-BR") }
            });

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");

            });
        }
    }
}
