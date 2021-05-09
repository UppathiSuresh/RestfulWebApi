using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RestWebApi.EmployeeData;
using RestWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestWebApi
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
            services.AddDbContextPool<EmployeeContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionString")));
            //services.AddScoped<IEmployeeData, SqlEmployeeData>();
            services.AddTransient<IEmployeeData, SqlEmployeeData>();
            services.AddControllers().AddJsonOptions(option=>
            {
                option.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
            });
            services.AddSwaggerGen(s=>
            {
                s.SwaggerDoc("v1",new Microsoft.OpenApi.Models.OpenApiInfo {Title="MY API", Version="v1" });

                s.AddSecurityDefinition("Bearer",new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description="JWT Authorization",
                    Name="Authorization",
                    In=Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Type=Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme="Bearer"
                }
                    );
            });


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer=Configuration["Jwt:Issuer"],
                    ValidAudience=Configuration["Jwt:Audience"],
                    IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
            s.SwaggerEndpoint("/swagger/v1/swagger.json", "My Web API v1");
            });
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(options =>options
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            );
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
