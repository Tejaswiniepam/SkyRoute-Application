using SkyRoute;
using SkyRoute.Models;

public interface IFlightSearchService
{
    IEnumerable<FlightSearchResponse> SearchFlights(
        string airline,
        string from,
        string to,
        DateTime departureDate,
        int passengers,
        CabinClass cabinClass);
}