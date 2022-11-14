using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VDT.Core.DependencyInjection;
using VDT.Core.DependencyInjection.Attributes;
using VDT.Core.DependencyInjection.Decorators;
using WarOfEmpires.CommandHandlers;
using WarOfEmpires.Database.Auditing;
using WarOfEmpires.QueryHandlers;
using WarOfEmpires.Middleware;
using WarOfEmpires.Services;
using WarOfEmpires.Utilities.Configuration;
using WarOfEmpires.Utilities.Events;
using WarOfEmpires.Utilities.Mail;
using WarOfEmpires.Configuration;

namespace WarOfEmpires {
    public class Startup {
        public Startup(IWebHostEnvironment env) {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .AddUserSecrets("c61bb83f-efb6-4b20-a8d9-eca92057923a");

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {
            services.AddControllersWithViews();
            services.AddHttpContextAccessor();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();
            // TODO investigate different authentication and authorization methods
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => options.LoginPath = "/Home/LogIn");
            services.AddServices(options => options
                .AddAssemblies(typeof(Startup).Assembly, nameof(WarOfEmpires))
                .AddServiceTypeProvider(DefaultServiceTypeProviders.InterfaceByName)
                .AddServiceTypeProvider(DefaultServiceTypeProviders.CreateGenericInterfaceTypeProvider(typeof(IEventHandler<>)))
                .AddServiceTypeProvider(DefaultServiceTypeProviders.CreateGenericInterfaceTypeProvider(typeof(IMailTemplate<>)))
                .AddAttributeServiceTypeProviders()
                .UseDefaultServiceLifetime(ServiceLifetime.Transient)
            );
            services.AddServices(options => options
                .AddAssemblies(typeof(Startup).Assembly, nameof(WarOfEmpires))
                .AddServiceTypeProvider(DefaultServiceTypeProviders.CreateGenericInterfaceTypeProvider(typeof(ICommandHandler<>)))
                .AddServiceTypeProvider(DefaultServiceTypeProviders.CreateGenericInterfaceTypeProvider(typeof(IQueryHandler<,>)))
                .UseDefaultServiceLifetime(ServiceLifetime.Transient)
                .UseDecoratorServiceRegistrar(decoratorOptions => decoratorOptions.AddDecorator<IAuditDecorator>())
            );

            services.AddSingleton(Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>());
            services.AddSingleton(Configuration.GetSection(nameof(ResourceSettings)).Get<ResourceSettings>());
            services.AddHostedService<ScheduledTaskRunnerService>();
            services.AddApplicationInsightsTelemetry();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<UserOnlineMiddleware>();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
