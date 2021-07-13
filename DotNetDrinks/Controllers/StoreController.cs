using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DotNetDrinks.Data;
using DotNetDrinks.Models;
using Microsoft.AspNetCore.Http;

namespace DotNetDrinks.Controllers
{
    public class StoreController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StoreController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Store
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.OrderBy(c => c.Name).ToListAsync());
        }

        public IActionResult Browse(int id)
        {
            // Use LINQ to query our DbSets/Collections
            // LINQ stands for Language Integrated Query
            // https://www.tutorialsteacher.com/linq/what-is-linq
            // https://linqsamples.com/linq-to-objects/aggregation
            // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/
            var products = _context.Products
                .Where(p => p.CategoryId == id) // Similar to WHERE in SQL
                .OrderBy(p => p.Name) // Similar to ORDER BY in SQL
                .ToList();

            // Get name of selected category
            ViewBag.category = _context.Categories.Find(id).Name;

            // Pass data to the View
            return View(products);
        }

        // Parameter names have to match input names
        public IActionResult AddToCart(int ProductId, int Quantity)
        {
            // query db to get product price
            var price = _context.Products.Find(ProductId).Price;

            // create and save Cart object
            var cart = new Cart
            {
                ProductId = ProductId,
                Quantity = Quantity,
                Price = price,
                DateCreated = DateTime.UtcNow, // Why is this important?
                // CustomerId = "TEST" // we will make this dynamic next
                CustomerId = GetCustomerId()
            };

            _context.Carts.Add(cart);
            _context.SaveChanges();

            // where to return
            return Redirect("Cart");
        }

        public IActionResult Cart()
        {
            // get CustomerId
            string customerId = GetCustomerId();
            // get current cart for display
            var cart = _context.Carts.Where(c => c.CustomerId == customerId).ToList();

            // return Content("Page under construction");
            return View(cart);
        }

        public string GetCustomerId()
        {
            // Check the session object for a CustomerId value
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("CustomerId")))
            {
                // No CustomerId value found
                string customerId = "";
                // Check if user is authenticated
                if (User.Identity.IsAuthenticated)
                {
                    // use email address as id
                    customerId = User.Identity.Name;
                }
                else
                {
                    // Anonymous user, use GUID as id
                    customerId = Guid.NewGuid().ToString();
                }
                // Set generated value as CustomerId
                HttpContext.Session.SetString("CustomerId", customerId);
            }
            // Return value in the session object
            return HttpContext.Session.GetString("CustomerId");
        }
    }
}
