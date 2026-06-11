using SkyRoute.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SkyRoute.Repository
{
    public class FlightRepository : IFlightRepository
    {
        private static readonly List<Flight> _flights = new()
        {
            new Flight 
            { 
                FlightId = 1, 
                Airline = "GlobalAir", 
                Origin = "NYC", 
                Destination = "LAX", 
                DepartureTime = DateTime.Now.AddDays(1), 
                ArrivalTime = DateTime.Now.AddDays(1).AddHours(5), 
                DistanceMiles = 2475,
                Cabins = new() 
                {
                    new CabinAvailability { CabinClass = CabinClass.Economy, SeatsAvailable = 50, PricePerPassenger = 0 },
                    new CabinAvailability { CabinClass = CabinClass.Bussiness, SeatsAvailable = 10, PricePerPassenger = 0 },
                    new CabinAvailability { CabinClass = CabinClass.FirstClass, SeatsAvailable = 4, PricePerPassenger = 0 }
                }
            },
            new Flight 
            { 
                FlightId = 2, 
                Airline = "BudgetWings", 
                Origin = "NYC", 
                Destination = "MIA", 
                DepartureTime = DateTime.Now.AddDays(1), 
                ArrivalTime = DateTime.Now.AddDays(1).AddHours(3), 
                DistanceMiles = 1280,
                Cabins = new() 
                {
                    new CabinAvailability { CabinClass = CabinClass.Economy, SeatsAvailable = 60, PricePerPassenger = 0 },
                    new CabinAvailability { CabinClass = CabinClass.Bussiness, SeatsAvailable = 8, PricePerPassenger = 0 },
                    new CabinAvailability { CabinClass = CabinClass.FirstClass, SeatsAvailable = 2, PricePerPassenger = 0 }
                }
            },
            new Flight 
            { 
                FlightId = 3, 
                Airline = "GlobalAir", 
                Origin = "LAX", 
                Destination = "ORD", 
                DepartureTime = DateTime.Now.AddDays(2), 
                ArrivalTime = DateTime.Now.AddDays(2).AddHours(4), 
                DistanceMiles = 1744,
                Cabins = new() 
                {
                    new CabinAvailability { CabinClass = CabinClass.Economy, SeatsAvailable = 50, PricePerPassenger = 0 },
                    new CabinAvailability { CabinClass = CabinClass.Bussiness, SeatsAvailable = 10, PricePerPassenger = 0 },
                    new CabinAvailability { CabinClass = CabinClass.FirstClass, SeatsAvailable = 4, PricePerPassenger = 0 }
                }
            },
            new Flight 
            { 
                FlightId = 4, 
                Airline = "BudgetWings", 
                Origin = "ORD", 
                Destination = "DFW", 
                DepartureTime = DateTime.Now.AddDays(3), 
                ArrivalTime = DateTime.Now.AddDays(3).AddHours(2), 
                DistanceMiles = 802,
                Cabins = new() 
                {
                    new CabinAvailability { CabinClass = CabinClass.Economy, SeatsAvailable = 60, PricePerPassenger = 0 },
                    new CabinAvailability { CabinClass = CabinClass.Bussiness, SeatsAvailable = 8, PricePerPassenger = 0 },
                    new CabinAvailability { CabinClass = CabinClass.FirstClass, SeatsAvailable = 2, PricePerPassenger = 0 }
                }
            },
            new Flight 
            { 
                FlightId = 5, 
                Airline = "GlobalAir", 
                Origin = "DFW", 
                Destination = "ATL", 
                DepartureTime = DateTime.Now.AddDays(4), 
                ArrivalTime = DateTime.Now.AddDays(4).AddHours(2), 
                DistanceMiles = 732,
                Cabins = new() 
                {
                    new CabinAvailability { CabinClass = CabinClass.Economy, SeatsAvailable = 50, PricePerPassenger = 0 },
                    new CabinAvailability { CabinClass = CabinClass.Bussiness, SeatsAvailable = 10, PricePerPassenger = 0 },
                    new CabinAvailability { CabinClass = CabinClass.FirstClass, SeatsAvailable = 4, PricePerPassenger = 0 }
                }
            },
            new Flight 
            { 
                FlightId = 6, 
                Airline = "BudgetWings", 
                Origin = "ATL", 
                Destination = "SFO", 
                DepartureTime = DateTime.Now.AddDays(5), 
                ArrivalTime = DateTime.Now.AddDays(5).AddHours(5), 
                DistanceMiles = 2139,
                Cabins = new() 
                {
                    new CabinAvailability { CabinClass = CabinClass.Economy, SeatsAvailable = 60, PricePerPassenger = 0 },
                    new CabinAvailability { CabinClass = CabinClass.Bussiness, SeatsAvailable = 8, PricePerPassenger = 0 },
                    new CabinAvailability { CabinClass = CabinClass.FirstClass, SeatsAvailable = 2, PricePerPassenger = 0 }
                }
            }
        };

        public List<Flight> GetAll() => _flights;

        public Flight GetById(int flightId)
        {
            return _flights.FirstOrDefault(f => f.FlightId == flightId);
        }
    }
}