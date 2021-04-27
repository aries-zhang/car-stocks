using System.ComponentModel.DataAnnotations;

using CarStocks.Common;

namespace CarStocks.Entities
{
    public class Car : IEntity
    {
        public int Id { get; set; }
        public int DealerId { get; set; }
        [Required]
        public string Make { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        [Range(1800, 2500)]
        public int Year { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }
    }
}
