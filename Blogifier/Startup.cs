using Blogifier.Providers.Context;
using Blogifier.Services;
using Blogifier.Services.Services;
using Blogifier.Shared.ViewModels.AppModels;
using Carwash.Core.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blogifier
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
            services.AddDbContext<AppDbContext>(options =>
              options.UseSqlServer(
                  Configuration.GetConnectionString("DefaultConnection")
                  // b => b.MigrationsAssembly(System.Reflection.Assembly.GetAssembly(typeof(Startup)).ToString())
                  ));

            services.AddCors(o => o.AddPolicy("BlogifierPolicy", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));

            RegisterDependencyInjection(services);

            #region JWT token
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var section = Configuration.GetSection("Blogifier");
                options.SaveToken = true;
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = section["Audience"],
                    ValidIssuer = section["Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(section["SymmetricSecurityKey"]))
                };
            });
            #endregion
            services.AddControllers();
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Blogifier", Version = "v1" });
            //});

            #region Swagger 
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                                   new OpenApiInfo
                                   {
                                       Title = "Blogifier",
                                       Version = "v1"
                                   });
                options.CustomSchemaIds(type => type.FullName);
                options.AddSecurityDefinition("Bearer",
                                              new OpenApiSecurityScheme
                                              {
                                                  Name = "Authorization",
                                                  Type = SecuritySchemeType.ApiKey,
                                                  In = ParameterLocation.Header,
                                                  Scheme = "oauth2",
                                                  Description =
                                                          "Please write the word 'Bearer' in the text field following by a space and a JWT token."
                                              });
                var apiSecurityRequirement = new OpenApiSecurityRequirement();
                apiSecurityRequirement.Add(new OpenApiSecurityScheme
                {
                    Name = "Bearer",
                    BearerFormat = string.Concat("Bearer "),
                    In = ParameterLocation.Header,
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    },
                    Scheme = "oauth2"
                },
                                           new List<string>());

                options.AddSecurityRequirement(apiSecurityRequirement);
                //options.SchemaFilter<SwaggerExcludeFilter>();
                options.SchemaFilter<EnumSchemaFilter>();
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var section = Configuration.GetSection("Blogifier");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blogifier v1"));
            }
            SeedDB.Initialize(app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope().ServiceProvider, section["SecurityKey"]);
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseExceptionHandler(c => c.Run(async context =>
            {
                var exception = context.Features.Get<IExceptionHandlerPathFeature>().Error;
                SaveException(app, exception);
                var response = new { error = exception.Message };
                await context.Response.WriteAsJsonAsync(response);
            }));
        }

        private void RegisterDependencyInjection(IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
        }
        private void SaveException(IApplicationBuilder app, Exception exception)
        {
            ExceptionLogsService instance = new ExceptionLogsService();

            instance.SaveExceptions(app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope().ServiceProvider,
                new ExceptionLogsModel()
                {
                    Message = exception.Message,
                    StackTrace = exception.StackTrace,
                    ErrorCode = exception.HResult.ToString(),
                });
        }
    }
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema model, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                model.Enum.Clear();
                Enum.GetNames(context.Type).ToList().ForEach(n => model.Enum.Add(new OpenApiString(n)));
            }
        }
    }
}
