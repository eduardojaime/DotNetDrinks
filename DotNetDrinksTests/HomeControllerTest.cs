using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNetDrinks.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDrinksTests
{
    // So far this is a regular C# class, how do we extend it to make it a test class?
    [TestClass]
    public class HomeControllerTest
    {
        // class is set up!
        [TestMethod]
        public void IndexReturnsResult()
        {
            // Arrange
            var controller = new HomeController(null);
            // Act
            var result = controller.Index();
            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PrivacyLoadsPrivacyView()
        {
            // Arrange
            var controller = new HomeController(null);
            // Act
            var result = (ViewResult)controller.Privacy();
            // Assert
            // result.ViewName is "Privacy"
            // TestCase Privacy method calls view named Privacy
            Assert.AreEqual("Privacy", result.ViewName);
        }
    }
}
