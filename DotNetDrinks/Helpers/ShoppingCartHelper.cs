using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNetDrinks.Helpers
{
    public class ShoppingCartHelper
    {
        private readonly IHttpContextAccessor _httpContext;
        protected readonly ISession _session;
        private ClaimsPrincipal _user;

        public ShoppingCartHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor;
            _session = _httpContext.HttpContext.Session;
            _user = _httpContext.HttpContext.User;
        }
        // Helper method > not designed to be used outside of this class
        public string GetCustomerId()
        {
            // Check the session object for a CustomerId value
            // Session will be persisted as long as the user remains on the page
            // Once browser is closed Session might be lost
            if (String.IsNullOrEmpty(_session.GetString("CustomerId")))
            {
                string customerId = "";
                // check if the user is authenticated and use email address as id
                if (_user.Identity.IsAuthenticated)
                {
                    customerId = _user.Identity.Name; // email address
                }
                else
                {
                    // or for anonymous users, generated a GUID and use that as id
                    customerId = Guid.NewGuid().ToString();
                }
                // Set generated value in my session object
                _session.SetString("CustomerId", customerId);
            }

            // return whatever is in the session object at this point
            return _session.GetString("CustomerId");
        }
    }
}
