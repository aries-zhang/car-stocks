using CarStocks.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarStocks.Entities
{
    public class Car : IEntity
    {
        public int Id { get; set; }
        public int DealerId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int Stock { get; set; }
    }
}
