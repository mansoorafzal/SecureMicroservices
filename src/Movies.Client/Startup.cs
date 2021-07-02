using Common;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Movies.Client.HttpHandlers;
using Movies.Client.Services;
using System;

namespace Movies.Client
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddScoped<IMovieService, MovieService>();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.Authority = Url.Identity_Server;
                options.ClientId = Constant.Movies_Client_Id_Value;
                options.ClientSecret = Constant.Movies_Client_Secret;
                options.ResponseType = Constant.Response_Type;

                options.Scope.Add(Constant.Scope_Open_Id);
                options.Scope.Add(Constant.Scope_Profile);
                options.Scope.Add(Constant.Scope_Address);
                options.Scope.Add(Constant.Scope_Email);
                options.Scope.Add(Constant.Scope_Role_Value);
                options.Scope.Add(Constant.Scope_Movie_Api_Value);

                options.ClaimActions.MapUniqueJsonKey(Constant.Scope_Role_Value, Constant.Scope_Role_Value);

                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = JwtClaimTypes.GivenName,
                    RoleClaimType = JwtClaimTypes.Role
                };
            });

            services.AddTransient<AuthenticationDelegatingHandler>();

            services.AddHttpClient(Constant.Http_Client_Movies_Api, client =>
            {
                client.BaseAddress = new Uri(Url.Api_Gateway);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add(HeaderNames.Accept, Constant.Content_Type_Json);
            })
            .AddHttpMessageHandler<AuthenticationDelegatingHandler>();

            services.AddHttpClient(Constant.Http_Client_Idp, client =>
            {
                client.BaseAddress = new Uri(Url.Identity_Server);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add(HeaderNames.Accept, Constant.Content_Type_Json);
            });

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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
