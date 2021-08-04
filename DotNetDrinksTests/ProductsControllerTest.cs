using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNetDrinks.Controllers;
using DotNetDrinks.Data;
using DotNetDrinks.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDrinksTests
{
    [TestClass]
    public class ProductsControllerTest
    {
        // connect to the DB?? create a mock object to simulate our db connection when testing
        // This is an In-Memory db context > Microsoft.EntityFrameworkCore.InMemory
        private ApplicationDbContext _context;
        // empty list of products
        List<Product> products = new List<Product>();
        // declare the controller that will be tested
        ProductsController controller;

        // How do I fill _context with data? or when?
        // Create a constructor?? Rather, create an Initialize method
        [TestInitialize]
        public void TestInitialize()
        {
            // instantiate in-memory db > similar to startup.cs
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            // create mock data in this db
            // Create 1 category
            var category = new Category { Id = 100, Name = "Test Category" };
            _context.Categories.Add(category);
            _context.SaveChanges();
            // Create 3 products
            products.Add(new Product { Id = 101, Name = "Product", Price = 11, Category = category });
            products.Add(new Product { Id = 102, Name = "Another Product", Price = 12, Category = category });
            products.Add(new Product { Id = 103, Name = "Extra Product", Price = 13, Category = category });

            foreach (var p in products)
            {
                _context.Products.Add(p);
            }
            _context.SaveChanges();

            // instanciate the controller class with mock db context
            controller = new ProductsController(_context);
        }



        // Tests that I need to write for archieving 100% coverage
        //Create(GET)
        //Create(POST)
        //Delete(GET)
        //DeleteConfirmed(POST)
        //Details
        //Edit(GET)
        //Edit(POST)
        //Index(GET)



    }
}
