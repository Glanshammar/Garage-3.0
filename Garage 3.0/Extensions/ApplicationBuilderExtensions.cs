﻿using Garage_3._0.Data;

namespace Garage_3._0.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task<IApplicationBuilder> SeedDataAsync(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDbContext>();

                try
                {
                    await SeedData.Init(context, services);
                    Console.WriteLine("Seed data initialized successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception during seed data initialization: {ex.Message}");
                }

            }
            return app;
        }
    }

}
