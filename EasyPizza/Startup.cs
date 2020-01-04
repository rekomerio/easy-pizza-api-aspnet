using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using EasyPizza.DAL;
using EasyPizza.Helpers;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using EasyPizza.Services;
using System;
using AutoMapper;

namespace EasyPizza
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private readonly IConfiguration _configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<EasyPizzaContext>(opt =>
               //opt.UseInMemoryDatabase("EasyPizzaList"));
               opt.UseSqlite("Data Source=LocalDatabase.db"));

            services.AddCors();
            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // configure strongly typed settings objects
            IConfiguration appSettingsSection = _configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            AppSettings appSettings = appSettingsSection.Get<AppSettings>();
            byte[] key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                 .AddJwtBearer(x =>
                 {
                     x.Events = new JwtBearerEvents
                     {
                         OnTokenValidated = context =>
                         {
                             var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                             // Read user id from token
                             var userId = int.Parse(context.Principal.Identity.Name);
                             // Check if the user exists
                             var user = userService.GetById(userId);
                             if (user == null)
                             {
                                 context.Fail("Unauthorized");
                             }
                             return Task.CompletedTask;
                         },
                     };
                     x.RequireHttpsMetadata = false;
                     x.SaveToken = true;
                     x.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuerSigningKey = true,
                         IssuerSigningKey = new SymmetricSecurityKey(key),
                         ValidateIssuer = false,
                         ValidateAudience = false,
                         ValidateLifetime = true
                     };
                 });

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRestaurantService, RestaurantService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP reques<4t pipeline.
        static public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Migrate any database changes on startup (includes initial db creation)
            using (IServiceScope scope = app.ApplicationServices.CreateScope())
                scope.ServiceProvider.GetService<EasyPizzaContext>().Database.Migrate();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(x => x
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
