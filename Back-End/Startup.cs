using AnL.Models;
using AnL.Repository.Abstraction;
using AnL.Repository.Implementation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestApplication
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
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                   builder => builder
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                        .SetIsOriginAllowed(origin => true) // allow any origin
               .AllowCredentials()); // allow credentials
            });
            services.AddControllers();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddDbContext<Tan_DBContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );
            services.AddHttpContextAccessor();
            services.AddSingleton<IUriService>(o =>
            {
                var accessor = o.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext.Request;
                var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new UriService(uri);
            });

            //var AuthenticationProviderKey = "Tangentia";
            ////var ValidIssuer = Configuration.GetSection("Jwt").GetSection("Issuer").Value;
            ////var ValidAudience = Configuration.GetSection("Jwt").GetSection("Audience").Value;
            ////var SecreteKey = Configuration.GetSection("Jwt").GetSection("SecretKey").Value;
            //var tokenValidationParameters = new TokenValidationParameters
            //{
            //    ValidateIssuer = true,
            //    ValidateAudience = true,
            //    ValidateLifetime = true,
            //    RequireSignedTokens = true,
            //    ValidateIssuerSigningKey = true,
            //    ValidIssuer = "Issuer",//Configuration["Jwt:Issuer"],
            //    ValidAudience = "Audience",//Configuration["Jwt:Audience"],
            //    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("KqcL7s998JrfFHRP"/*_configuration["Jwt:SecretKey"]*/)),
            //    ClockSkew = TimeSpan.FromMinutes(20),
            //    RequireExpirationTime = true,
            //    LifetimeValidator = (DateTime? notBefore, DateTime? expires, SecurityToken securityToken,
            //                         TokenValidationParameters validationParameters) =>
            //    {
            //        return notBefore <= DateTime.UtcNow &&
            //               expires >= DateTime.UtcNow;
            //    }
            //};


            //services.AddAuthentication(options =>
            //{
            //    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //}).AddJwtBearer(config =>
            //{
            //    config.RequireHttpsMetadata = false;
            //    config.SaveToken = true;
            //    config.TokenValidationParameters = tokenValidationParameters;
            //    config.Authority = "https://localhost:44351";
            //});

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
            options => Configuration.Bind("JwtSettings", options))
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
        options => Configuration.Bind("CookieSettings", options));
            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "A&L Timesheet Management",
                    Version = "v1.1",
                    Description = ""

                });
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                swagger.IncludeXmlComments(xmlPath);
            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("CorsPolicy");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "A&L Timesheet Management");
                c.DefaultModelsExpandDepth(-1);
            });
            var option = new RewriteOptions();
            option.AddRedirect("^$", "swagger");
            app.UseRewriter(option);
        }
    }
}
