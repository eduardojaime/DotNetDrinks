using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNetDrinks.Controllers;
using DotNetDrinks.Data;
using DotNetDrinks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace BeerStoreTests
{
    [TestClass]
    public class ProductsControllerTest
    {
        // Set up mock data
        private ApplicationDbContext _context;
        // empty list of products
        List<Product> products = new List<Product>();
        // declare controller we are going to test
        ProductsController controller;

        [TestInitialize]
        public void TestInitialize()
        {
            // instantiate in-memory db > similar to initialization in startup.cs
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            // create mock data inside in-memory db
            var category = new Category { Id = 100, Name = "Test Category" };
            _context.Categories.Add(category);
            _context.SaveChanges();

            products.Add(new Product { Id = 101, Name = "One Test 1", Price = 11, Category = category });
            products.Add(new Product { Id = 102, Name = "Another Test 2", Price = 12, Category = category });
            products.Add(new Product { Id = 103, Name = "Extra Test 3", Price = 13, Category = category });

            foreach (var p in products)
            {
                _context.Products.Add(p);
            }
            _context.SaveChanges();

            // instantiate controller class and pass it the mock db object
            controller = new ProductsController(_context);
        }

        // To achieve 100% code coverage (is 100% necessary??)
        // Create(GET)
        // Create(POST)
        // Delete(GET)
        // DeleteConfirmed(POST)
        // Details
        // Edit(GET)
        // Edit(POST)
        // Index(GET)
        [TestMethod]
        public void IndexViewLoads()
        {
            // arrange step done in TestInitialize()
            // Perform operations
            var result = controller.Index();
            var viewResult = (ViewResult)result.Result;
            // Assert
            Assert.AreEqual("Index", viewResult.ViewName);
        }

        public void IndexReturnsProductData()
        {
            var result = controller.Index();
            var viewResult = (ViewResult)result.Result;
            // Assert that list of products displayed on index is the same as the list we have
            List<Product> model = (List<Product>)viewResult.Model;
            // Assert lists are equal
            CollectionAssert.AreEqual(products, model);
        }
    }
}
