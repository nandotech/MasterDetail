using MasterDetail.DataLayer;
using MasterDetail.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MasterDetail.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            var userManager =
                new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            userManager.UserValidator = new UserValidator<ApplicationUser>(userManager)
                                        {
                                            AllowOnlyAlphanumericUserNames = false
                                        };

            var roleManager =
                new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(new ApplicationDbContext()));

            string name = "pluralsightnimda@gmail.com";
            string password = "Pluralsight#1";
            string firstName = "Admin";
            string roleName = "Admin";

            var role = roleManager.FindByName(roleName);

            if (role == null)
            {
                role = new ApplicationRole(roleName);
                var roleResult = roleManager.Create(role);
            }

            var user = userManager.FindByName(name);

            if (user == null)
            {
                user = new ApplicationUser { UserName = name, Email = name, FirstName = firstName };
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            var rolesForUser = userManager.GetRoles(user.Id);

            if (!rolesForUser.Contains(role.Name))
            {
                var result = userManager.AddToRole(user.Id, role.Name);
            }

            ////////////////////////////////////////////////////

            string categoryName = "Housing";

            context.Categories.AddOrUpdate(
                c => c.CategoryName,
                new Category { CategoryName = categoryName });

            context.SaveChanges();

            Category category = context.Categories.First(c => c.CategoryName == categoryName);

            context.Categories.AddOrUpdate(
                c => c.CategoryName,
                new Category { CategoryName = "Furniture", ParentCategoryId = category.Id },
                new Category { CategoryName = "Fixtures", ParentCategoryId = category.Id },
                new Category { CategoryName = "Building Materials", ParentCategoryId = category.Id }
                );

            categoryName = "Learning Materials";

            context.Categories.AddOrUpdate(
                c => c.CategoryName,
                new Category { CategoryName = categoryName });

            context.SaveChanges();

            category = context.Categories.First(c => c.CategoryName == categoryName);

            context.Categories.AddOrUpdate(
                c => c.CategoryName,
                new Category { CategoryName = "Books", ParentCategoryId = category.Id },
                new Category { CategoryName = "Supplies", ParentCategoryId = category.Id }
                );

            context.Categories.AddOrUpdate(
                c => c.CategoryName,
                new Category { CategoryName = "Food and Water" });

            context.SaveChanges();

            category = context.Categories.First(c => c.CategoryName == "Housing");

            context.InventoryItems.AddOrUpdate(
                ii => ii.InventoryItemName,
                new InventoryItem { CategoryId = category.Id, InventoryItemCode = "CLASSROOM", InventoryItemName = "Pre-Fabricated Classroom", UnitPrice = 10000m }
                );

            category = context.Categories.First(c => c.CategoryName == "Fixtures");

            context.InventoryItems.AddOrUpdate(
                ii => ii.InventoryItemName,
                new InventoryItem { CategoryId = category.Id, InventoryItemCode = "WHITEBOARD", InventoryItemName = "Whiteboard", UnitPrice = 324.50m },
                new InventoryItem { CategoryId = category.Id, InventoryItemCode = "ARMOR", InventoryItemName = "Armor Plating Kit", UnitPrice = 1225m }
                );

            category = context.Categories.First(c => c.CategoryName == "Building Materials");

            context.InventoryItems.AddOrUpdate(
                ii => ii.InventoryItemName,
                new InventoryItem { CategoryId = category.Id, InventoryItemCode = "CONCRETE", InventoryItemName = "Concrete, 50 lbs.", UnitPrice = 12.05m },
                new InventoryItem { CategoryId = category.Id, InventoryItemCode = "REBAR", InventoryItemName = "Rebar", UnitPrice = 3.50m }
                );

            category = context.Categories.First(c => c.CategoryName == "Furniture");

            context.InventoryItems.AddOrUpdate(
                ii => ii.InventoryItemName,
                new InventoryItem { CategoryId = category.Id, InventoryItemCode = "STUDENTDESK", InventoryItemName = "Student Desk", UnitPrice = 18.75m },
                new InventoryItem { CategoryId = category.Id, InventoryItemCode = "TEACHERDESK", InventoryItemName = "Teacher Desk", UnitPrice = 60m },
                new InventoryItem { CategoryId = category.Id, InventoryItemCode = "CHAIR", InventoryItemName = "Chair", UnitPrice = 9.65m }
                );

            category = context.Categories.First(c => c.CategoryName == "Books");

            context.InventoryItems.AddOrUpdate(
                ii => ii.InventoryItemName,
                new InventoryItem { CategoryId = category.Id, InventoryItemCode = "SCIENCETEXT", InventoryItemName = "Science Textbook", UnitPrice = 30.25m },
                new InventoryItem { CategoryId = category.Id, InventoryItemCode = "ARTTEXT", InventoryItemName = "Art History Textbook", UnitPrice = 41m },
                new InventoryItem { CategoryId = category.Id, InventoryItemCode = "POETRYTEXT", InventoryItemName = "Greatest Poems of All Time", UnitPrice = 15.95m }
                );

            category = context.Categories.First(c => c.CategoryName == "Supplies");

            context.InventoryItems.AddOrUpdate(
                ii => ii.InventoryItemName,
                new InventoryItem { CategoryId = category.Id, InventoryItemCode = "STUDENTSUP", InventoryItemName = "Student School Supplies Kit", UnitPrice = 12m },
                new InventoryItem { CategoryId = category.Id, InventoryItemCode = "TEACHERSUP", InventoryItemName = "Teacher School Supplies Kit", UnitPrice = 35m }
                );

            context.ServiceItems.AddOrUpdate(
                si => si.ServiceItemName,
                new ServiceItem { ServiceItemCode = "FORMANDPOUR", ServiceItemName = "Form and Pour Foundation", Rate = 35.50m },
                new ServiceItem { ServiceItemCode = "ERECTPREFAB", ServiceItemName = "Erect Pre-Fabricated Classroom", Rate = 47m },
                new ServiceItem { ServiceItemCode = "DIGWELL", ServiceItemName = "Dig Well and Install Hand Pump", Rate = 30m },
                new ServiceItem { ServiceItemCode = "INSTALLARMOR", ServiceItemName = "Install Armor Plating", Rate = 63.75m }
                );

            context.Customers.AddOrUpdate(
                cu => cu.AccountNumber,
                new Customer { AccountNumber = "GSTEMS", CompanyName = "Girls STEM School", Address = "35 Achievement Way", City = "Detroit", State = "MI", ZipCode = "48223", Phone = "123-456-7890" },
                new Customer { AccountNumber = "YWLS", CompanyName = "Young Women's Literary Society", Address = "1523 Aruna Lane", City = "Milwaukee", State = "WI", ZipCode = "53202", Phone = "234-567-8901" },
                new Customer { AccountNumber = "TRS", CompanyName = "The Roosevelt School", Address = "731 Kramer Street", City = "Philadelphia", State = "PA", ZipCode = "19115", Phone = "345-678-9012" }
                );
        }
    }
}
