using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectPlutus.Data.Context;
using ProjectPlutus.Domain.Models;
using ProjectPlutus.Infra;
using ProjectPlutus.Infra.Repositories;

namespace ProjectPlutus.Extensions
{
    public static class DependencyExtensions
    {
        public static void AddDomainDataServices(
            this IServiceCollection services,
            string connectionString)
        {
            services.AddDbContext<ProjectPlutusContext>(
                o => o.UseSqlServer(connectionString));

            services.AddTransient<IGenericRepository<Employee>, EmployeeRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }

        public static void AddSwaggerService(
            this IApplicationBuilder app)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProjectPlutus API V1");
            });
        }
    }
}
