using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using MyLearning.Services;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;

namespace MyLearning
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
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options
                .SetIsOriginAllowed(_ => true)
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());
                //.WithOrigins("http://localhost:52403", "http://localhost:4200"));
            });

            services.AddResponseCompression();

            services.AddControllers().AddNewtonsoftJson(
                opt =>
                {
                    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            services.AddDbContext<MyLearningDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("MyLearningDbContext")));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

            // configures services for multipartbodylength
            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });

            // formats model state error message to custom message
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState.Where(
                        e => e.Value.Errors.Count > 0
                        ).Select(e => new
                        {
                            Error = e.Value.Errors.First().ErrorMessage,

                        }).ToArray();
                    return new BadRequestObjectResult(new
                    {
                        Status = false,
                        Message = errors,
                        Data = new { }
                    });
                };
            });


            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "My Learning API",
                    Description = "An ASP.NET Core Web API for Chinice" +
                    " App extending the functionality of Ebusiness. The project was done in asp.net core 3.1",
                    TermsOfService = new Uri("http://www.elixirprimehouse.com/"),
                    Contact = new OpenApiContact
                    {
                        Name = "Elixir Prime House",
                        Email = "info@elixirprimehouse.com",
                        Url = new Uri("http://www.elixirprimehouse.com/"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Elixir Prime House",
                        Url = new Uri("http://www.elixirprimehouse.com/"),
                    }
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
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
                        In = ParameterLocation.Header
                    },
                    new string[] { }
                }
                });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseResponseCompression();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "Elixir Prime House");
            });

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors("AllowOrigin");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
