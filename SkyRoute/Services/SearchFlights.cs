using SkyRoute.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;

namespace SkyRoute.Services
{
    public class FlightService
    {
        private readonly IFlightRepository _flightRepo;
        private readonly ILogger<FlightService> _logger;

        public FlightService(IFlightRepository flightRepo, ILogger<FlightService> logger)
        {
            _flightRepo = flightRepo;
            _logger = logger;
        }

        public List<Flight> GetAvailableFlights(
            string from,
            string to,
            DateTime departureDate,
            int passengers,
            CabinClass cabinClass)
        {
            _logger.LogInformation(
                "GetAvailableFlights called: From={From}, To={To}, Date={Date}, Passengers={Passengers}, Cabin={Cabin}",
                from, to, departureDate, passengers, cabinClass);
            return _flightRepo
                .GetAll()
                .Where(f =>
                    f.Origin == from &&
                    f.Destination == to &&
                    f.DepartureTime.Date == departureDate.Date &&
                    f.Cabins.Any(c =>
                        c.CabinClass == cabinClass &&
                        c.SeatsAvailable >= passengers
                    )
                )
                .ToList();
        }
    }
}