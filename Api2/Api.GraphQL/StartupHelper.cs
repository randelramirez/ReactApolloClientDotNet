using System.Linq;
using Api.GraphQL.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Api.GraphQL
{
    public static class StartupHelper
    {
        public static void SeedDataContext(this IApplicationBuilder application)
        {
            using (var serviceScope = application.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope())
            {
                if (serviceScope != null)
                {
                    var context = serviceScope.ServiceProvider.GetRequiredService<DataContext>();

                    context.Database.EnsureCreated();

                    var databaseSeeder = new DataInitializer(context);
                    if (!context.Sessions.Any())
                    {
                        databaseSeeder.SeedDatabase();
                    }
                }
            }
        }
    }
}