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

        // GET: /Store
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.OrderBy(c => c.Name).ToListAsync());
        }

        // GET: /Store/Browse/<id>
        public IActionResult Browse(int id)
        {
            // Use context object to query the database and get a list of products by categoryId
            // Use LINQ
            // https://www.tutorialsteacher.com/linq/what-is-linq
            // https://linqsamples.com/linq-to-objects/aggregation
            // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/
            // vs SQL >> string query = "SELECT * FROM PRODUCTS WHERE CATEGORYID = @CATID";
            var products = _context.Products
                           .Where(p => p.CategoryId == id)
                           .OrderBy(p => p.Name)
                           .ToList();

            // how else can I send data back to the view?
            // ViewBag.category = _context.Categories.Where(c => c.Id == id).FirstOrDefault().Name;
            ViewBag.category = _context.Categories.Find(id).Name;

            // pass the list to be used as a model to the view
            return View(products);
        }

        // Add input names as parameters for ASP.NET to bind them automatically
        // Model binder
        // Parameter values are coming from input fields names must match
        public IActionResult AddToCart(int ProductId, int Quantity)
        {
            // query db to get product price, use LINQ
            var price = _context.Products.Find(ProductId).Price;

            // get or generate a customerid
            string customerId = GetCustomerId();

            // create and save cart object
            var cart = new Cart()
            {
                ProductId = ProductId,
                Quantity = Quantity,
                Price = price,
                DateCreated = DateTime.UtcNow, // returns date time in UTC timezone
                CustomerId = customerId
            };

            _context.Carts.Add(cart);
            _context.SaveChanges();

            // redirect to Cart view          
            return Redirect("Cart");
        }

        public IActionResult Cart()
        {
            string customerId = GetCustomerId();
            // Use LINQ to query the Carts collection
            // Cart is a list of Products
            var cart = _context.Carts.Where(c => c.CustomerId == customerId).ToList();
            return View(cart);
        }

        // Helper method > not designed to be used outside of this class
        private string GetCustomerId()
        {
            // Check the session object for a CustomerId value
            // Session will be persisted as long as the user remains on the page
            // Once browser is closed Session might be lost
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("CustomerId")))
            {
                string customerId = "";
                // check if the user is authenticated and use email address as id
                if (User.Identity.IsAuthenticated)
                {
                    customerId = User.Identity.Name; // email address
                }
                else
                {
                    // or for anonymous users, generated a GUID and use that as id
                    customerId = Guid.NewGuid().ToString();
                }
                // Set generated value in my session object
                HttpContext.Session.SetString("CustomerId", customerId);
            }

            // return whatever is in the session object at this point
            return HttpContext.Session.GetString("CustomerId");
        }
    }
}
