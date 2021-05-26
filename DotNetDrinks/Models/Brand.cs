using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetDrinks.Models
{
    public class Brand
    {
        // in ASP.NET, key fields should always be called either Id or {Model}Id
        public int Id { get; set; }
        public string Name { get; set; }
        public int YearFounded { get; set; }
    }
}
