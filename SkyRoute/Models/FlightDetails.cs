using System;

namespace SkyRoute
{
    public class FlightSearchResult
    {
        public int FlightId { get; set; }

        public string Airline { get; set; } = string.Empty;

        public DateTime DepartureTime { get; set; }

        public DateTime ArrivalTime { get; set; }

        public int DistanceMiles { get; set; }

        public decimal BaseFare { get; set; }

        public decimal Adjustment { get; set; }

        public decimal FinalFare { get; set; }

        public bool MinPriceApplied { get; set; }
    }

    public class Flight
    {
        public int FlightId { get; set; }

        public string? Airline { get; set; }

        public string? Origin { get; set; }

        public string? Destination { get; set; }

        public DateTime DepartureTime { get; set; }

        public DateTime ArrivalTime { get; set; }

        public int DistanceMiles { get; set; }

        public List<CabinAvailability> Cabins { get; set; } = new();
    }

    public class CabinAvailability
    {
        public CabinClass CabinClass { get; set; }

        public int SeatsAvailable { get; set; }

        public decimal PricePerPassenger { get; set; }
    }

    public enum CabinClass
    {
        Economy,
        Bussiness,
        FirstClass
    }
}