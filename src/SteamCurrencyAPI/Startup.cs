using Microsoft.OpenApi.Models;
using Quartz;
using Quartz.AspNetCore;
using SteamCurrencyAPI.DataWrapper;
using SteamCurrencyAPI.Interfaces;
using SteamCurrencyAPI.Jobs;
using SteamCurrencyAPI.Services;
using System.Reflection;

namespace SteamCurrencyAPI;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Steam Currency API",
                Version = "v1",
                Description = @"
### EN
An API for getting Steam currency exchange rates. 
Provides the latest rates, historical data for a selected period, and a list of all available currency codes.
This data can be useful for applications that work with converting prices for Steam items.

### RU
API для получения курсов обмена валют Steam. 
Предоставляет актуальные курсы, исторические данные за выбранный период и список всех доступных кодов валют.
Эти данные могут быть полезны для приложений, которые работают с конвертацией цен на предметы в Steam.
",
                Contact = new OpenApiContact
                {
                    Name = "Author",
                    Url = new Uri("https://github.com/NikGob")
                },
                License = new OpenApiLicense
                {
                    Name = "MIT License",
                    Url = new Uri("https://opensource.org/licenses/MIT"),
                }
            });
            c.EnableAnnotations();

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });

        services.AddHttpContextAccessor();
        services.AddMemoryCache();

        services.AddSingleton<ICurrencyDbContext, CurrencyDbContext>();

        services.AddScoped<ICurrencyGetValueService, CurrencyGetValueService>();
        services.AddScoped<ICurrencyService, CurrencyService>();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.WithMethods("POST", "GET", "PATCH", "PUT")
                    .AllowAnyOrigin()
                    .AllowAnyHeader();
            });
        });

        services.AddQuartz(q =>
        {
            var jobKey = new JobKey("CurrencyUpdaterJob");
            q.AddJob<CurrencyUpdaterJob>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("CurrencyUpdaterJob-trigger")
                .WithCronSchedule("0 10 0 * * ?", x => x
                    .InTimeZone(TimeZoneInfo.Utc))
            );
        });

        services.AddQuartzServer(options =>
        {
            options.WaitForJobsToComplete = true;
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseHsts();
        }

        app.UseRouting();

        app.UseCors("AllowAll");

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute().AllowAnonymous();
            endpoints.MapSwagger();
            endpoints.MapControllers().AllowAnonymous();
        });
    }
}