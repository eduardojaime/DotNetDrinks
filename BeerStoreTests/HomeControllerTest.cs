using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNetDrinks.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BeerStoreTests
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void IndexReturnsResult()
        {
            // Arrange Data
            // Send null parameter
            var controller = new HomeController(null);
            // Perform operations
            var result = controller.Index();
            // Assert result (fail or pass)
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PrivacyLoadsPrivacyView()
        {
            // Arrange data
            var controller = new HomeController(null);
            // Perform operations 
            // Cast result to a view result object
            var result = (ViewResult)controller.Privacy();
            // Check name of view
            Assert.AreEqual("Privacy", result.ViewName);
        }
    }
}
