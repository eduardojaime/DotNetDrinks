using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNetDrinks.Controllers;
using DotNetDrinks.Data;
using DotNetDrinks.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
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

            // Create 1 brand
            var brand = new Brand { Id = 100, Name = "No Name" };
            _context.Brands.Add(brand);
            _context.SaveChanges();

            // Create 3 products
            products.Add(new Product { Id = 101, Name = "Product", Price = 11, Category = category, Brand = brand });
            products.Add(new Product { Id = 102, Name = "Another Product", Price = 12, Category = category, Brand = brand });
            products.Add(new Product { Id = 103, Name = "Extra Product", Price = 13, Category = category, Brand = brand });

            foreach (var p in products)
            {
                _context.Products.Add(p);
            }
            _context.SaveChanges();

            // instanciate the controller class with mock db context
            controller = new ProductsController(_context);
        }

        // Tests that I need to write for archieving 100% coverage
        // Create(GET)
        [TestMethod]
        public void GetCreateView()
        {
            // Arrange > InitializeTest()
            // Act
            // returns ResultView directly since it's not asynch
            // cast object to ViewResult
            var result = (ViewResult)controller.Create();
            // Assert that this is the correct view
            Assert.AreEqual("Create", result.ViewName);

        }
        // Create(POST)
        [TestMethod]
        public void PostCreateProduct()
        {
            // Arrange > InitializeTest()
            // Act
            // Create a new product
            Product newProd = new Product { 
                Name = "TequilaTest", 
                Price = 100,
                Stock = 10,
                BrandId= 100, 
                CategoryId = 100
            };
            // Asynch method returns wrapper around result object
            var result = controller.Create(newProd, null);
            // Assert that product is in DB
            // Select by name
            var prod = _context.Products
                .Where(p => p.Name == newProd.Name)
                .FirstOrDefault();
            // if found then prod is not null
            Assert.IsNotNull(prod);
        }
        // Delete(GET)
        // DeleteConfirmed(POST)
        // Details
        [TestMethod]
        public void GetDetailsNotFoundWithNulId()
        {
            // Arrange > InitializeTest()
            // Act
            var result = controller.Details(null);
            var notFoundResult = (NotFoundResult)result.Result;
            // Assert that status code returned is 404 Not Found
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }
        // Edit(GET)
        // Edit(POST)

        // Index(GET)
        [TestMethod]
        public void IndexViewLoads()
        {
            // Arrange
            // we can skip it since this is done in TestInitialize()
            // Act
            var result = controller.Index();
            var viewResult = (ViewResult)result.Result;
            // Assert
            Assert.AreEqual("Index", viewResult.ViewName);
        }

        [TestMethod]
        public void IndexReturnsProductData()
        {
            // Act
            // Call index action method and cast result
            var result = controller.Index();
            var viewResult = (ViewResult)result.Result;
            // Extract list of product generated in the controller
            var model = (List<Product>)viewResult.Model;
            // Match ordering specified in product controller
            var orderedProducts = products.OrderBy(p => p.Name).ToList();
            // Assert both lists are equal
            CollectionAssert.AreEqual(orderedProducts, model);
        }

        // ProductExists



    }
}
