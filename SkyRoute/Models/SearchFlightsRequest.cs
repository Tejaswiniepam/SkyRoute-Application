using System.ComponentModel.DataAnnotations;

namespace SkyRoute.Models
{
    public class SearchFlightsRequest
    {
        [Required]
        public string From { get; set; } = string.Empty;

        [Required]
        public string To { get; set; } = string.Empty;

        [Required]
        public DateTime DepartureDate { get; set; }

        [Range(1, 9)]
        public int Passengers { get; set; }

        [Required]
        public CabinClass CabinClass { get; set; }
    }
}