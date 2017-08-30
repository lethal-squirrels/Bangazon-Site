using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Bangazon.Models;
using System.Threading.Tasks;

namespace Bangazon.Data
{
    // Class to seed our database with data for testing purposes.
    public static class DbInitializer
    {
        // Method runs on startup to initialize dummy data.
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {

                //Checks if the table is already seeded and breaks if it is
                              if (context.ProductType.Any())
                              {
                                       return;
                              }
                // Creating new instances of ProductType
                var productTypes = new ProductType[]
                {
                    new ProductType {
                        Label = "Grocery"
                    },
                     new ProductType {
                        Label = "Auto"
                    },
                    new ProductType {
                        Label = "Home"
                    },
                    new ProductType {
                        Label = "Bath"
                    },
                    new ProductType {
                        Label = "Bedroom ;)"
                    },
                    new ProductType {
                        Label = "Electronics"
                    },
                    new ProductType {
                        Label = "Clothing"
                    },
                    new ProductType {
                        Label = "Office"
                    },
                    new ProductType {
                        Label = "Jewlery"
                    },
                    new ProductType {
                        Label = "Nutrition"
                    },
                };

                // Adds each new product type into the context
                foreach (ProductType p in productTypes)
                {
                    context.ProductType.Add(p);
                }
                // Saves the ApplicationUsers to the database
                context.SaveChanges();

            }
        }
    }
}