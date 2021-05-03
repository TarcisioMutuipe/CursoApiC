using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SmartSchool.API.Data;

namespace SmartSchool.API
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
            services.AddDbContext<SmartContext>(
                context => context.UseMySql(Configuration.GetConnectionString("MySqlConnection"))
                );

            //services.AddSingleton<IRepository, Repository>();
            // services.AddTransient<IRepository, Repository>();
           
            services.AddControllers().AddNewtonsoftJson(
                opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IRepository, Repository>();

            services.AddSwaggerGen(
                opt =>
                {
                opt.SwaggerDoc(
                    "SmartSchoolAPI",
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title=" SmartSchool API",
                        Version = "1.0",
                        TermsOfService= new Uri("http://Tarcisio.com"),
                        Description = "A Escola do Futuro",
                        License = new Microsoft.OpenApi.Models.OpenApiLicense()
                        { 
                            Name = "Smart License",
                            Url= new Uri("http://mit.com")
                        },
                        Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                        { 
                            Name = "Tarcisio",
                            Email  ="tarcisiusantos@gmail.com"                            
                        },
                   
                       
                });
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = "SmartSchool API",
                        Version = "1.0"
                    };
                    var xmlCommentario = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlComentarioCaminho = Path.Combine(AppContext.BaseDirectory, xmlCommentario);

                    opt.IncludeXmlComments(xmlComentarioCaminho);
                });
                services.AddCors();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors(x=> x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseSwagger()
                .UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/SmartSchoolAPI/swagger.json", "smartschoolapi");
                    options.RoutePrefix = "";

                });
                
          //  app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
