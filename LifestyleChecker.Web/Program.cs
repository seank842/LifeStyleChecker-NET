using LifestyleChecker.Web.Components;
using LifestyleChecker.Web.Services.Authentication;
using LifestyleChecker.Web.Services.Authentication.States;
using MudBlazor.Services;

namespace LifestyleChecker.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add service defaults & Aspire client integrations.
            builder.AddServiceDefaults();
            builder.AddRedisOutputCache("cache");

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            // Add UI components and services.
            builder.Services.AddScoped<Radzen.TooltipService>();
            builder.Services.AddMudServices();

            // Add authentication and authorization services.
            builder.Services.AddSingleton<AdminAuthState>();
            builder.Services.AddSingleton<PatientAuthState>();
            builder.Services.AddTransient<AdminAuthHandler>();
            builder.Services.AddTransient<PatientAuthHandler>();

            // Add HTTP clients for API communication.
            builder.Services.AddHttpClient("adminAPI", client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]);
            }).AddHttpMessageHandler<AdminAuthHandler>();

            builder.Services.AddHttpClient("patientAPI", client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]);
            }).AddHttpMessageHandler<PatientAuthHandler>();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error", createScopeForErrors: true);
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.UseOutputCache();

            app.MapStaticAssets();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.MapDefaultEndpoints();

            app.Run();

        }
    }
}