namespace SkyRoute.Models
{
    public class FlightSearchResponse
    {
        public int FlightId { get; init; }

        public string Airline { get; init; } = string.Empty;

        public DateTime DepartureTime { get; init; }

        public DateTime ArrivalTime { get; init; }

        public int DistanceMiles { get; init; }

        public decimal BaseFare { get; init; }

        public decimal Adjustment { get; init; }

        public decimal FinalFare { get; init; }
    }
}