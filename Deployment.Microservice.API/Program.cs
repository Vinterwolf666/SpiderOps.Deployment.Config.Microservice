
using Deployment.Microservice.APP;
using Deployment.Microservice.Infrastructure;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;

namespace Deployment.Microservice.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            

            builder.Services.AddControllers();
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            var configuration = builder.Configuration;

            builder.Services.AddDbContext<PipelinesDBContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("Value"), b => b.MigrationsAssembly("Deployment.Microservice.API")));
            
            builder.Services.AddDbContext<CustomPipelinesDBContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("Value"), b => b.MigrationsAssembly("Deployment.Microservice.API")));


            builder.Services.AddScoped<IPipelinesRepository, PipelinesRepository>();
            builder.Services.AddScoped<IPipelinesServices, PipelinesServices>();

           
            builder.Services.AddScoped<ICustomerPipelinesRepository, CustomPipelinesRepository>();
            builder.Services.AddScoped<ICustomPipelinesServices, CustomPipelinesServices>();

          

           

         
            

            builder.Services.AddCors(options =>
            {

                options.AddPolicy("nuevaPolitica", app =>
                {

                    app.AllowAnyOrigin();
                    app.AllowAnyHeader();
                    app.AllowAnyMethod();
                });



            });

            var app = builder.Build();

           
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("nuevaPolitica");
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}