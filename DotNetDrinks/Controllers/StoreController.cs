﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DotNetDrinks.Data;
using DotNetDrinks.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using DotNetDrinks.Extensions;

using Stripe;
using Microsoft.Extensions.Configuration;

namespace DotNetDrinks.Controllers
{
    public class StoreController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IConfiguration _configuration;

        public StoreController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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
            var cart = _context.Carts.Include(c => c.Product).Where(c => c.CustomerId == customerId).ToList();
            return View(cart);
        }

        [Authorize]
        public IActionResult Checkout()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout([Bind("Address,City,Province,PostalCode")] Models.Order order)
        {
            // populate 3 automatic order properties
            order.OrderDate = DateTime.UtcNow;
            order.CustomerId = User.Identity.Name;

            // calculate order total
            var cartCustomerId = GetCustomerId();
            var cartItems = _context.Carts
                            .Where(c => c.CustomerId == cartCustomerId)
                            .ToList();

            order.Total = cartItems.Sum(c => c.Price);

            // Use SessionsExtension object to store the order object
            HttpContext.Session.SetObject("Order", order);

            return RedirectToAction("Payment");
        }

        public IActionResult RemoveFromCart(int id)
        {
            var cartItem = _context.Carts.Where(c => c.Id == id).FirstOrDefault();

            if (cartItem != null)
            {
                _context.Carts.Remove(cartItem);
                _context.SaveChanges();
            }
            return RedirectToAction("Cart");
        }

        public IActionResult Payment()
        {
            // Get our order
            var order = HttpContext.Session.GetObject<Models.Order>("Order");
            // Stripe amount must be in cents
            ViewBag.Total = order.Total * 100;
            // Use configuration to read key from appsettings
            ViewBag.PublishableKey = _configuration["Stripe:PublishableKey"];

            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Payment(string stripeToken)
        {
            // create stripe customer

            // create stripe charge

            // save a new order to our db

            // save cart items as new orderdetails to our db

            // delete cart items from order

            // load order confirmation page

            // redirect
            return View();
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
