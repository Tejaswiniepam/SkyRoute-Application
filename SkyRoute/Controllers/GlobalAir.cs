using Microsoft.AspNetCore.Mvc;
using SkyRoute;

[ApiController]
[Route("api/[controller]")]
public class GlobalAirFlights : ControllerBase
{
    private readonly IFlightSearchService _flightSearchService;

    public GlobalAirFlights(IFlightSearchService flightSearchService)
    {
        _flightSearchService = flightSearchService;
    }

    [HttpGet]
    public IActionResult Search(
        [FromQuery] string from,
        [FromQuery] string to,
        [FromQuery] DateTime departureDate,
        [FromQuery] int passengers = 1,
        [FromQuery] CabinClass cabinClass = CabinClass.Economy)
    {
        var result = _flightSearchService.SearchFlights(
            "GlobalAir",
            from,
            to,
            departureDate,
            passengers,
            cabinClass);

        return result.Any()
            ? Ok(result)
            : NotFound("No flights available.");
    }
}