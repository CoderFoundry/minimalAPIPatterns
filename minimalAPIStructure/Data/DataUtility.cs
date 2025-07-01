
using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using minimalAPIStructure.Models;
using MinimalAPIStructure.Data;
using MinimalAPIStructure.Models;

namespace AuthDemoYT.Data
{
    public static class DataUtility
    {
        private static readonly List<string> MarvelRelics = new()
    {
        "Tesseract (Space Stone)",
        "Aether (Reality Stone)",
        "Orb of Power (Power Stone)",
        "Mind Stone Scepter",
        "Eye of Agamotto (Time Stone)",
        "Soul Stone",
        "Infinity Gauntlet",
        "Casket of Ancient Winters",
        "Ebony Blade",
        "Book of Vishanti",
        "Ultimate Nullifier",
        "Cosmic Cube",
        "Quantum Bands",
        "Serpent Crown",
        "Gungnir (Spear of Odin)",
        "Mjölnir",
        "Stormbreaker",
        "Jarnbjörn",
        "Muramasa Blade",
        "Norn Stones"
    };

        private static readonly Dictionary<string, string> RelicDescriptions = new()
        {
            ["Tesseract (Space Stone)"] = "An ancient cube housing the Space Stone, capable of teleporting matter anywhere in the universe.",
            ["Aether (Reality Stone)"] = "A fluid-like primordial force that can rewrite reality at the wielder’s will.",
            ["Orb of Power (Power Stone)"] = "An orb containing the Power Stone, granting immense strength and energy manipulation.",
            ["Mind Stone Scepter"] = "A scepter infused with the Mind Stone, used to control minds and amplify mental powers.",
            ["Eye of Agamotto (Time Stone)"] = "A mystical amulet housing the Time Stone, able to bend and loop time itself.",
            ["Soul Stone"] = "An enigmatic gem that can manipulate souls and open gateways to the spiritual realm.",
            ["Infinity Gauntlet"] = "A golden glove designed to harness all six Infinity Stones simultaneously.",
            ["Casket of Ancient Winters"] = "An Asgardian ice chest that emits unending cold and can freeze entire armies.",
            ["Ebony Blade"] = "A sentient sword forged from a demon’s tooth, nearly unbreakable and thirsts for blood.",
            ["Book of Vishanti"] = "The White Sorcerer’s tome of pure magic, containing the most potent spells in the multiverse.",
            ["Ultimate Nullifier"] = "A device of last resort, capable of erasing entire universes with a single thought.",
            ["Cosmic Cube"] = "A device that transforms thought into reality on a cosmic scale.",
            ["Quantum Bands"] = "Armor worn by Quasar that manipulates quantum energy for flight and force blasts.",
            ["Serpent Crown"] = "An ancient crown granting its wearer control over sea creatures and dark magic.",
            ["Gungnir (Spear of Odin)"] = "Odin’s enchanted spear, unerringly accurate and imbued with Asgardian power.",
            ["Mjölnir"] = "Thor’s legendary hammer that can only be lifted by those deemed worthy.",
            ["Stormbreaker"] = "Thor’s new axe, able to summon the Bifrost and channel lightning.",
            ["Jarnbjörn"] = "An ancient battleaxe once wielded by Thor, forged from uru metal.",
            ["Muramasa Blade"] = "A cursed katana created by the master swordsmith Muramasa, grants enhanced abilities.",
            ["Norn Stones"] = "A trio of stones that grant the bearer control over fate and destiny."
        };


        public static string? GetConnectionString(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DbConnection");
            return string.IsNullOrEmpty(connectionString) ? null : connectionString;
        }


        public static async Task ManageDataAsync(IServiceProvider svcProvider)
        {
            //Service: An instance of RoleManager
            await using var dbContextSvc = svcProvider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>().CreateDbContext();

            //Service: An IConfiguration instance to get appsettings/secrets/environment variables
            var configurationSvc = svcProvider.GetRequiredService<IConfiguration>();
            //Migration: This is the programmatic equivalent to Update-Database
            await dbContextSvc.Database.MigrateAsync();


            await SeedDemoCustomersAsync(dbContextSvc);
            await SeedDemoProductsAsync(dbContextSvc);
            await SeedDemoOrdersAsync(dbContextSvc);

            await dbContextSvc.DisposeAsync();
        }




        public static async Task SeedDemoCustomersAsync(ApplicationDbContext context, int count = 50)
        {
            try
            {
                if (context.Customers.Any())
                {
                    // If customers already exist, skip seeding
                    return;
                }
                // Generate fake customers using Bogus
                var customers = GenerateCustomers(count);
                context.Customers.AddRange(customers);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("*************  ERROR  *************");
                Console.WriteLine("Error Seeding Customers.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }
        }

        public static async Task SeedDemoProductsAsync(ApplicationDbContext context, int count = 50)
        {
            try
            {
                if (context.Products.Any())
                {
                    // If products already exist, skip seeding
                    return;
                }
                // Generate fake products using Bogus
                var productFaker = new Faker<Product>()
                    .RuleFor(p => p.Id, f => 0) // EF will assign Id
                    .RuleFor(p => p.Name, f => f.PickRandom(MarvelRelics))            
                    .RuleFor(p => p.Description, (f, p) => RelicDescriptions[p.Name])
                    .RuleFor(p => p.Price, f => Math.Round(f.Random.Decimal(1, 500), 2));
                var products = productFaker.Generate(count);
                context.Products.AddRange(products);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("*************  ERROR  *************");
                Console.WriteLine("Error Seeding Products.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }
        }

        public static async Task SeedDemoOrdersAsync(ApplicationDbContext context, int count = 100)
        {

            try
            {
                if (context.Orders.Any())
                {
                    // If orders already exist, skip seeding
                    return;
                }
                // Ensure we have customers to assign orders to
                if (!context.Customers.Any())
                {
                    throw new InvalidOperationException("Cannot seed orders without customers. Please seed customers first.");
                }
                var customers = await context.Customers.ToListAsync();
                var products = await context.Products.ToListAsync();
                var orders = GenerateOrders(customers, products, count);
                context.Orders.AddRange(orders);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("*************  ERROR  *************");
                Console.WriteLine("Error Seeding Orders.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }
        }

        /// <summary>
        /// Generates a list of fake Customer entities.
        /// </summary>
        /// <param name="count">Number of customers to generate.</param>
        public static List<Customer> GenerateCustomers(int count)
        {
            var id = 1;
            var customerFaker = new Faker<Customer>()
                // incrementing index for Id
                .RuleFor(c => c.Id, f => id++)
                .RuleFor(c => c.FirstName, f => f.Name.FirstName())
                .RuleFor(c => c.LastName, f => f.Name.LastName())
                .RuleFor(c => c.Email, (f, c) => f.Internet.Email(c.FirstName, c.LastName))
                .RuleFor(c => c.Address, f => f.Address.StreetAddress())
                .RuleFor(c => c.Address2, f => f.Random.Bool(0.2f) ? f.Address.SecondaryAddress() : null)
                .RuleFor(c => c.City, f => f.Address.City())
                .RuleFor(c => c.State, f => f.Address.StateAbbr())
                .RuleFor(c => c.ZipCode, f => f.Address.ZipCode("#####"));

            return customerFaker.Generate(count);
        }

        /// <summary>
        /// Generates a list of fake Order entities associated with provided customers.
        /// </summary>
        /// <param name="customers">List of customers to assign orders to.</param>
        /// <param name="count">Number of orders to generate.</param>
        public static List<Order> GenerateOrders(IList<Customer> customers, IList<Product> products, int count)
        {
            var orderFaker = new Faker<Order>()
                .RuleFor(o => o.Id, f => 0) // EF will assign Id

                .RuleFor(o => o.Customer, f => f.PickRandom(customers))
                .RuleFor(o => o.CustomerId, (f, o) => o.Customer.Id)

                .RuleFor(o => o.Product, f => f.PickRandom(products))
                .RuleFor(o => o.ProductId, (f, o) => o.Product.Id)

                .RuleFor(o => o.UnitPrice, f => Math.Round(f.Random.Decimal(1, 500), 2))
                .RuleFor(o => o.Quantity, f => f.Random.Int(1, 20))
                .RuleFor(o => o.OrderDate, f => f.Date.Past(1))
                .RuleFor(o => o.DeliveryDate, (f, o) =>
                {
                    // ensure DeliveryDate is a business day within 30 days of OrderDate
                    var date = f.Date.Between(o.OrderDate, o.OrderDate.AddDays(30));
                    while (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                    {
                        date = date.AddDays(1);
                    }
                    return date;
                });

            var orders = orderFaker.Generate(count);
            return orders;
        }

    }

}
