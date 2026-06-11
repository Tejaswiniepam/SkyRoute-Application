using SkyRoute;
using SkyRoute.Models;
using SkyRoute.Pricing;
using SkyRoute.Services;
using Microsoft.Extensions.Logging;

public class FlightSearchService : IFlightSearchService
{
    private readonly FlightService _flightService;
    private readonly IFareCalculator _fareCalculator;
    private readonly ILogger _logger;

    public FlightSearchService(
        FlightService flightService,
        IFareCalculator fareCalculator,
        ILogger logger)
    {
        _flightService = flightService;
        _fareCalculator = fareCalculator;
        _logger = logger;
    }

    public IEnumerable<FlightSearchResponse> SearchFlights(
        string airline,
        string from,
        string to,
        DateTime departureDate,
        int passengers,
        CabinClass cabinClass)
    {
        _logger.LogInformation(
         "Search flights response: Passengers={Passengers}, Airline={Airline}, From={From}, To={To}, DepartureDate={DepartureDate}, CabinClass={CabinClass}",
         passengers, airline, from, to, departureDate, cabinClass);
        return _flightService
            .GetAvailableFlights(from, to, departureDate, passengers, cabinClass)
            .Where(f => f.Airline == airline)
            .Select(f => new FlightSearchResponse
            {
                FlightId = f.FlightId,
                Airline = f.Airline,
                DepartureTime = f.DepartureTime,
                ArrivalTime = f.ArrivalTime,
                DistanceMiles = f.DistanceMiles,
                BaseFare = _fareCalculator.CalculateBaseFare(
                    f.DistanceMiles,
                    cabinClass),
                Adjustment = _fareCalculator.CalculateAdjustment(
                    airline,
                    f.DistanceMiles,
                    cabinClass),
                FinalFare = _fareCalculator.CalculateFinalFare(
                    airline,
                    f.DistanceMiles,
                    cabinClass)
            });
    }
}