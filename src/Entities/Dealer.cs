using CarStocks.Common;

namespace CarStocks.Entities
{
    public class Dealer : IEntity
    {
        public int Id { get; set; }

        public string Token { get; set; }
    }
}
