using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Bangazon.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Bangazon.Data
{
    // Class to seed our database with data for testing purposes.
    public static class DbInitializer
    {
        // Method runs on startup to initialize dummy data.
        public async static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {


                //Checks if the table is already seeded and breaks if it is
                if (context.ProductType.Any())
                {
                    return;
                }

                var userStore = new UserStore<ApplicationUser>(context);

                var userList = new ApplicationUser[]
                {
                    new ApplicationUser {
                        UserName = "a@a.com",
                        NormalizedUserName = "A@A.COM",
                        Email = "a@a.com",
                        NormalizedEmail = "A@A.com",
                        Phone = "617-391-3918",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        SecurityStamp = Guid.NewGuid().ToString("D"),
                        ZipCode = "37202",
                        FirstName = "Bob",
                        LastName = "Smith",
                        StreetAddress = "902 Irksome Lane",
                        City = "Brentwood",
                        State = "NY"
                    },
                    new ApplicationUser {
                        UserName = "b@b.com",
                        NormalizedUserName = "B@B.COM",
                        Email = "b@b.com",
                        NormalizedEmail = "B@B.com",
                        Phone = "213-481-5829",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        SecurityStamp = Guid.NewGuid().ToString("D"),
                        ZipCode = "92812",
                        FirstName = "Jenny",
                        LastName = "Frazier",
                        StreetAddress = "1904 Bonnie Lane",
                        City = "Edinburgh",
                        State = "WV"
                    },
                    new ApplicationUser
                    {
                        UserName = "c@c.com",
                        NormalizedUserName = "C@C.COM",
                        Email = "c@c.com",
                        NormalizedEmail = "C@C.com",
                        Phone = "924-271-0482",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        SecurityStamp = Guid.NewGuid().ToString("D"),
                        ZipCode = "09182",
                        FirstName = "Tinker",
                        LastName = "Bell",
                        StreetAddress = "283 Bell Rd",
                        City = "NeverNeverLand",
                        State = "OH"
                    }
                };

                foreach (ApplicationUser user in userList)
                {
                    var password = new PasswordHasher<ApplicationUser>();
                    var hashed = password.HashPassword(user, "password");
                    user.PasswordHash = hashed;
                    await userStore.CreateAsync(user);
                }
                await context.SaveChangesAsync();

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
                        Label = "Electronics"
                    },
                    new ProductType {
                        Label = "Clothing"
                    }
                };

                // Adds each new product type into the context
                foreach (ProductType p in productTypes)
                {
                    context.ProductType.Add(p);
                }
                // Saves the ApplicationUsers to the database
                await context.SaveChangesAsync();

                var products = new Product[]
                    {
                        new Product {
                            Name = "Banana",
                            Description = "Yummy and yellow",
                            ImgPath = "/images/bananas.jpg",
                            Price = .99,
                            Quantity = 15,
                            ProductTypeID = context.ProductType.Single(t => t.Label == "Grocery").ProductTypeID,
                            DateCreated = DateTime.Now,
                            User = userStore.Users.First<ApplicationUser>(user => user.UserName == "a@a.com"),
                            Location = "Nashville"
                    },
                        new Product {
                            Name = "40 inch rims",
                            Description = "Only for Big Ballers",
                            ImgPath = "/images/rims.jpg",
                            Price = 55,
                            Quantity = 2,
                            ProductTypeID = context.ProductType.Single(t => t.Label == "Auto").ProductTypeID,
                            DateCreated = DateTime.Now,
                            User = userStore.Users.First<ApplicationUser>(user => user.UserName == "a@a.com"),
                            Location = "Nashville",
                    },
                        new Product {
                            Name = "Roof shingles",
                            Description = "Take cover",
                            ImgPath = "/images/shingles.jpg",
                            Price = 4,
                            Quantity = 150,
                            ProductTypeID = context.ProductType.Single(t => t.Label == "Home").ProductTypeID,
                            DateCreated = DateTime.Now,
                            User = userStore.Users.First<ApplicationUser>(user => user.UserName == "a@a.com"),
                             Location = "",

                    },
                        new Product {
                            Name = "Bath mat",
                            Description = "Cold floors are the worst",
                            ImgPath = "/images/bath-mat.jpg",
                            Price = 25.50,
                            Quantity = 9,
                            ProductTypeID = context.ProductType.Single(t => t.Label == "Home").ProductTypeID,
                            DateCreated = DateTime.Now,
                            User = userStore.Users.First<ApplicationUser>(user => user.UserName == "b@b.com"),
                            Location = "Chicago"
                    },
                        new Product {
                            Name = "King size bed",
                            Description = "Fit for a king. Or whatever.",
                            ImgPath = "/images/bed.jpg",
                            Price = 1200,
                            Quantity = 1,
                            ProductTypeID = context.ProductType.Single(t => t.Label == "Home").ProductTypeID,
                            DateCreated = DateTime.Now,
                            User = userStore.Users.First<ApplicationUser>(user => user.UserName == "b@b.com"),
                            Location = "Chicago"
                    },
                        new Product {
                            Name = "Nintendo 64",
                            Description = "Obviously the best gaming system",
                            ImgPath = "/images/nintendo.jpg",
                            Price = 75,
                            Quantity = 3,
                            ProductTypeID = context.ProductType.Single(t => t.Label == "Electronics").ProductTypeID,
                            DateCreated = DateTime.Now,
                            User = userStore.Users.First<ApplicationUser>(user => user.UserName == "b@b.com"),
                            Location = "",

                    },
                        new Product {
                            Name = "Shoes",
                            Description = "They're shoes. They shoe your feet.",
                            ImgPath = "/images/shoes.jpg",
                            Price = 5,
                            Quantity = 6,
                            ProductTypeID = context.ProductType.Single(t => t.Label == "Clothing").ProductTypeID,
                            DateCreated = DateTime.Now,
                            User = userStore.Users.First<ApplicationUser>(user => user.UserName == "b@b.com"),
                            Location = "",
                    },
                        new Product {
                            Name = "Fruit roll ups",
                            Description = "They ship better than bananas, trust me",
                            ImgPath = "/images/fruit-roll-up.jpg",
                            Price = 1.50,
                            Quantity = 1000,
                            ProductTypeID = context.ProductType.Single(t => t.Label == "Grocery").ProductTypeID,
                            DateCreated = DateTime.Now,
                            User = userStore.Users.First<ApplicationUser>(user => user.UserName == "c@c.com"),
                            Location = "Miami"
                    },
                        new Product {
                            Name = "Doritos",
                            Description = "Yummy and orange",
                            ImgPath = "/images/doritos.jpg",
                            Price = 3.75,
                            Quantity = 15,
                            ProductTypeID = context.ProductType.Single(t => t.Label == "Grocery").ProductTypeID,
                            DateCreated = DateTime.Now,
                            User = userStore.Users.First<ApplicationUser>(user => user.UserName == "c@c.com"),
                            Location = "Miami"
                    },
                        new Product {
                            Name = "Headphones",
                            Description = "Hear things better",
                            ImgPath = "/images/headphones.jpg",
                            Price = 15.15,
                            Quantity = 3,
                            ProductTypeID = context.ProductType.Single(t => t.Label == "Electronics").ProductTypeID,
                            DateCreated = DateTime.Now,
                            User = userStore.Users.First<ApplicationUser>(user => user.UserName == "a@a.com"),
                            Location = "",
                    },
                        new Product {
                            Name = "Comforter",
                            Description = "For the king size bed. Or whatever.",
                            ImgPath = "/images/comforter.jpg",
                            Price = 67,
                            Quantity = 2,
                            ProductTypeID = context.ProductType.Single(t => t.Label == "Home").ProductTypeID,
                            DateCreated = DateTime.Now,
                            User = userStore.Users.First<ApplicationUser>(user => user.UserName == "b@b.com"),
                            Location = "",
                    },
                        new Product {
                            Name = "Semi-truck",
                            Description = "Drive in style",
                            ImgPath = "/images/semi-truck.jpg",
                            Price = 150000,
                            Quantity = 1,
                            ProductTypeID = context.ProductType.Single(t => t.Label == "Auto").ProductTypeID,
                            DateCreated = DateTime.Now,
                            User = userStore.Users.First<ApplicationUser>(user => user.UserName == "c@c.com"),
                            Location = "",
                    },
                        new Product {
                            Name = "Socks",
                            Description = "They go with your shoes",
                            ImgPath = "/images/socks.jpg",
                            Price = 4.75,
                            Quantity = 1200,
                            ProductTypeID = context.ProductType.Single(t => t.Label == "Clothing").ProductTypeID,
                            DateCreated = DateTime.Now,
                            User = userStore.Users.First<ApplicationUser>(user => user.UserName == "a@a.com"),
                            Location = "",
                    }
                };

                // Adds each new product into the context
                foreach (Product p in products)
                {
                    context.Product.Add(p);
                }
                // Saves the products to the database
                await context.SaveChangesAsync();

                // Creating new instances of ProductType
                var paymentTypes = new PaymentType[]
                {
                    new PaymentType {
                        User = userStore.Users.First<ApplicationUser>(user => user.UserName == "a@a.com"),
                        Description = "Visa",
                        AccountNumber = "9923498123120",
                        DateCreated = DateTime.Now,
                        IsActive = true,
                    },
                     new PaymentType {
                        User = userStore.Users.First<ApplicationUser>(user => user.UserName == "a@a.com"),
                        Description = "Checking Account",
                        AccountNumber = "12983423914",
                        DateCreated = DateTime.Now,
                        IsActive = true,
                    },
                      new PaymentType {
                        User = userStore.Users.First<ApplicationUser>(user => user.UserName == "b@b.com"),
                        Description = "Mastercard",
                        AccountNumber = "1204812945",
                        DateCreated = DateTime.Now,
                        IsActive = true,
                    },
                       new PaymentType {
                        User = userStore.Users.First<ApplicationUser>(user => user.UserName == "b@b.com"),
                        Description = "Paypal",
                        AccountNumber = "9923498123120",
                        DateCreated = DateTime.Now,
                        IsActive = true,
                    },
                        new PaymentType {
                        User = userStore.Users.First<ApplicationUser>(user => user.UserName == "c@c.com"),
                        Description = "Savings Account",
                        AccountNumber = "30459239845234",
                        DateCreated = DateTime.Now,
                        IsActive = true,
                    },
                        new PaymentType {
                        User = userStore.Users.First<ApplicationUser>(user => user.UserName == "c@c.com"),
                        Description = "Amex",
                        AccountNumber = "287346378193482",
                        DateCreated = DateTime.Now,
                        IsActive = true,
                    }

                };

                // Adds each new product type into the context
                foreach (PaymentType p in paymentTypes)
                {
                    context.PaymentType.Add(p);
                }
                // Saves the ApplicationUsers to the database
                await context.SaveChangesAsync();


                // Creating new instances of ProductType
                var orders = new Order[]
                {
                    new Order {
                        User = userStore.Users.First<ApplicationUser>(user => user.UserName == "a@a.com"),
                        PaymentTypeID = context.PaymentType.Single(t => t.Description == "Visa").PaymentTypeID,
                        DateCreated = DateTime.Now
                    },
                    new Order {
                        User = userStore.Users.First<ApplicationUser>(user => user.UserName == "a@a.com"),
                        PaymentTypeID = context.PaymentType.Single(t => t.Description == "Visa").PaymentTypeID,
                        DateCreated = DateTime.Now
                    },
                    new Order {
                        User = userStore.Users.First<ApplicationUser>(user => user.UserName == "b@b.com"),
                        PaymentTypeID = context.PaymentType.Single(t => t.Description == "Paypal").PaymentTypeID,
                        DateCreated = DateTime.Now
                    },
                    new Order {
                        User = userStore.Users.First<ApplicationUser>(user => user.UserName == "c@c.com"),
                        PaymentTypeID = context.PaymentType.Single(t => t.Description == "Amex").PaymentTypeID,
                        DateCreated = DateTime.Now
                    }
                };

                // Adds each new product type into the context
                foreach (Order o in orders)
                {
                    context.Order.Add(o);
                }
                // Saves the ApplicationUsers to the database
                await context.SaveChangesAsync();

                var lineItems = new ProductOrder[]
                {
                    new ProductOrder {
                        OrderID = 1,
                        ProductID = context.Product.Single(t => t.Name == "Shoes").ProductID,
                    },
                    new ProductOrder {
                        OrderID = 1,
                        ProductID = context.Product.Single(t => t.Name == "Bath mat").ProductID,
                    },
                    new ProductOrder {
                        OrderID = 2,
                        ProductID = context.Product.Single(t => t.Name == "King size bed").ProductID,
                    },
                    new ProductOrder {
                        OrderID = 2,
                        ProductID = context.Product.Single(t => t.Name == "Semi-truck").ProductID,
                    },
                    new ProductOrder {
                        OrderID = 3,
                        ProductID = context.Product.Single(t => t.Name == "Doritos").ProductID,
                    },
                    new ProductOrder {
                        OrderID = 3,
                        ProductID = context.Product.Single(t => t.Name == "Fruit roll ups").ProductID,
                    },
                    new ProductOrder {
                        OrderID = 4,
                        ProductID = context.Product.Single(t => t.Name == "Nintendo 64").ProductID,
                    },
                    new ProductOrder {
                        OrderID = 4,
                        ProductID = context.Product.Single(t => t.Name == "Socks").ProductID,
                    }
                };
                // Adds each new product type into the context
                foreach (ProductOrder p in lineItems)
                {
                    context.ProductOrder.Add(p);
                }
                // Saves the ApplicationUsers to the database
                await context.SaveChangesAsync();

            };
        }
    }
}