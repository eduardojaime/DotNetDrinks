using DotNetDrinks.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetDrinks.Controllers
{
    public class BrandsControllerOld : Controller
    {
        public IActionResult Index()
        {
            // use Brand model to create 5 mock brands to pass to the view for display
            var brands = new List<Brand>();

            brands.Add(new Brand { Id = 100, Name = "Canadian Club", YearFounded = 1902 });
            brands.Add(new Brand { Id = 101, Name = "Molson", YearFounded = 1786 });
            brands.Add(new Brand { Id = 102, Name = "Glenfiddich", YearFounded = 1883 });
            brands.Add(new Brand { Id = 103, Name = "JP Wiser", YearFounded = 1853 });
            brands.Add(new Brand { Id = 104, Name = "Jackson Triggs", YearFounded = 1993 });

            // pass the brands list to the view
            return View(brands);
        }

        // GET: /Brands/Details?name=Some Brand
        public IActionResult Details(string name)
        {
            if (name == null)
            {
                return BadRequest();
            }

            ViewBag.name = name;
            return View();
        }

        // GET: /Brands/Create
        public IActionResult Create()
        {
            return View();
        }
    }
}
