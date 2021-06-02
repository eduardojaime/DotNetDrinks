using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetDrinks.Models
{
    public class Brand
    {
        // in ASP.NET, key fields should always be called either Id or {Model}Id
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Range(1400, 2025)]
        public int YearFounded { get; set; }
    }
}
