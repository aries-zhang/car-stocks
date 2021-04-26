using CarStocks.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarStocks.Entities
{
    public class Dealer : IEntity
    {
        public int Id { get; set; }

        public string Token { get; set; }
    }
}
