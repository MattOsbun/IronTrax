using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Irontrax.Models;
using Irontrax.Services.Facades;
using Irontrax.Services.Identity;
using Irontrax.Services.Interfaces;
using Irontrax.WebApplication.Wrappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Irontrax.WebApplication
{
    public class Startup
    {
        private static HttpClient _httpClient = BuildHttpClient();
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            configuration = builder.Build();

            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(1800);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddControllersWithViews();

            services.AddIdentity<IrontraxUser, ApplicationRole>();
            services.ConfigureApplicationCookie(options => options.LoginPath = "/Authenticate/Login");
            services.AddScoped<IUserClaimsPrincipalFactory<IrontraxUser>, IrontraxUserclaimsPrincipalFactory>();
            services.AddSingleton<IUserStore<IrontraxUser>>(CreateUserStore());
            services.AddSingleton<IRoleStore<ApplicationRole>>(CreateRoleStore());
            services.AddSingleton<IActivityManageable>(BuildActivitiesFacade());
            services.AddHttpContextAccessor();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();
            app.UseHttpContext();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private IrontraxRoleStore CreateRoleStore()
        {
            return new IrontraxRoleStore(); //TODO: Properly implement 
        }

        private static HttpClient BuildHttpClient()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
            var httpClient = new HttpClient(handler);
            httpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
            httpClient.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("gzip"));
            httpClient.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("deflate"));
            return httpClient;
        }

        private ActivitiesFacade BuildActivitiesFacade()
        {
            return new ActivitiesFacade(
                _httpClient,
                Configuration.GetValue<string>("AppSettings:ActivityApiBaseUrl")
            );
        }

        private IrontraxUserStore CreateUserStore()
        {
            IUserManageable userManager = new IdentityFacade(
                _httpClient,
                Configuration.GetValue<string>("AppSettings:IdentityApiBaseUrl")
            );
            return new IrontraxUserStore(userManager);
        }
    }

    public static class HttpContextExtensions
    {
        public static void AddHttpContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public static IApplicationBuilder UseHttpContext(this IApplicationBuilder app)
        {
            SessionWrapper.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
            return app;
        }
    }
}
