using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using PolyclinicsSystemBackend.Config;
using PolyclinicsSystemBackend.Config.Options;
using PolyclinicsSystemBackend.Data;
using PolyclinicsSystemBackend.Data.Entities;
using PolyclinicsSystemBackend.Middleware;
using Autofac;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.Logging;
using PolyclinicsSystemBackend.Dtos.Account.Authorize;

namespace PolyclinicsSystemBackend
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
            services.AddDbContext<AppDbContext>(options =>
              options.UseNpgsql(Configuration.GetConnectionString("Default")));

            services.AddIdentity<User, IdentityRole>( c =>
                    c.User.RequireUniqueEmail = true)
              .AddEntityFrameworkStores<AppDbContext>()
              .AddDefaultTokenProviders();
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = Configuration["Auth:Issuer"],
                        ValidAudience = Configuration["Auth:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Auth:Key"])),
                        ClockSkew = TimeSpan.Zero, // remove delay of token when expire
                    };
                });
            services.AddAuthorization(options =>
            {
                var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
                    JwtBearerDefaults.AuthenticationScheme);

                defaultAuthorizationPolicyBuilder =
                    defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();

                options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
            });
            services.Configure<AuthOptions>(Configuration.GetSection("Auth"));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "PolyclinicsSystemBackend API", Version = "v1"});
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            services
              .AddControllers()
              .AddFluentValidation(fv =>
              {
                  fv.ImplicitlyValidateChildProperties = true;
                  fv.RegisterValidatorsFromAssemblyContaining<Startup>();
              })
              .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AppDbContext dbContext, IServiceProvider serviceProvider, ILogger<Startup> logger)
        { 
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseEndpoints(endpoints =>
              endpoints.MapControllerRoute("default", "{controller}/{action=Index}/{id?}"));
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PolyclinicsSystemBackend API V1"));
            dbContext.Database.Migrate();
            CreateRoles(serviceProvider, logger).Wait();
        }
        
        private async Task CreateRoles(IServiceProvider serviceProvider, ILogger<Startup> logger)
        {
            //initializing custom roles 
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            string[] roleNames = { "Admin", "Doctor", "Patient" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    //create the roles and seed them to the database: Question 1
                    var result = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            //Here you could create a super user who will maintain the web app
            var adminUser = new User
            {

                Email = Configuration["AdminUserEmail"],
                UserName = Configuration["AdminUserName"]
            };
            //Ensure you have these values in your appsettings.json file
            var adminUserPassword = Configuration["AdminUserPassword"];
            var user = await userManager.FindByEmailAsync(adminUser.Email);

            if(user == null)
            {
                var createPowerUser = await userManager.CreateAsync(adminUser, adminUserPassword);
                if (createPowerUser.Succeeded)
                {
                    //here we tie the new user to the role
                   foreach (var roleName in roleNames)
                    {
                        var result = await userManager.AddToRoleAsync(adminUser, roleName);
                        if (!result.Succeeded)
                            logger.LogWarning("Failed to bind {Role} role to admin user", roleName);
                    }
                }
                else
                {
                    logger.LogWarning("Failed to create admin user");
                }
            }
        }
    }
}