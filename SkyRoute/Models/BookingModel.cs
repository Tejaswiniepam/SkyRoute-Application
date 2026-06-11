using System;
using System.ComponentModel.DataAnnotations;

namespace SkyRoute.Models
{
    public class PassengerDetails
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; } = string.Empty;

        [Required]
        public string DocumentNumber { get; set; } = string.Empty;
    }

    public class BookingRequest
    {
        public int FlightId { get; set; }

        [Required(ErrorMessage = "Cabin class is mandatory. Please select Economy, Bussiness, or FirstClass.")]
        public CabinClass CabinClass { get; set; }

        [Required]
        public List<PassengerDetails> Passengers { get; set; } = new();

        // Derived from list — never sent from frontend
        public int NumberOfPassengers => Passengers?.Count ?? 0;
    }

    public class BookingConfirmation
    {
        public string BookingReference { get; set; } = string.Empty;

        public decimal TotalPrice { get; set; }

        public decimal FarePerPerson { get; set; }

        public DateTime BookingDate { get; set; }
    }
}