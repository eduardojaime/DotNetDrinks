using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetDrinks.Controllers
{
    public class BrandsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Brands/Details?name=Some Brand
        public IActionResult Details(string name)
        {
            ViewBag.name = name;
            return View();
        }
    }
}
