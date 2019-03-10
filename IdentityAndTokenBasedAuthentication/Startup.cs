using System.Text;
using IdentityAndTokenBasedAuthentication.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace IdentityAndTokenBasedAuthentication
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

            //UserManager
            services.AddTransient<UserManager<ApplicationUser>>();

            //SignInManager
            services.AddTransient<SignInManager<ApplicationUser>>();

            //Add Token validation Parameters
            var tokenParams = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = Configuration["JWT:issuer"],
                ValidAudience = Configuration["JWT:audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:key"]))

            };

            //Add JWT Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(jwtconfig =>
                {
                    jwtconfig.TokenValidationParameters = tokenParams;
                });

            //Connection String
            services.AddDbContext<CustomIdentityDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //Application User
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<CustomIdentityDbContext>().AddDefaultTokenProviders();

            //Api Json Format
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.Formatting = Formatting.Indented;
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
