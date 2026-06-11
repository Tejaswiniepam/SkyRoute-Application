using Microsoft.Extensions.Logging;
using SkyRoute.Exceptions;
using SkyRoute.Models;
using SkyRoute.Pricing;
using SkyRoute.Repository;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SkyRoute.Services
{
    public interface IBookingService
    {
        BookingResponse BookFlight(BookingRequest request);
    }

    public class BookingService : IBookingService
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IFareCalculator _fareCalculator;
        private readonly ILogger<BookingService> _logger;

        public BookingService(
            IFlightRepository flightRepository,
            IFareCalculator fareCalculator,
            ILogger<BookingService> logger)
        {
            _flightRepository = flightRepository;
            _fareCalculator = fareCalculator;
            _logger = logger;
        }

        public BookingResponse BookFlight(BookingRequest request)
        {
            _logger.LogInformation(
                "Booking requested: FlightId={FlightId}, CabinClass={CabinClass}, PassengerCount={PassengerCount}",
                request.FlightId, request.CabinClass, request.NumberOfPassengers);

            if (request.Passengers == null || request.Passengers.Count == 0)
            {
                _logger.LogWarning("Booking failed: no passengers provided for FlightId={FlightId}", request.FlightId);
                throw new ValidationException("At least one passenger is required.");
            }

            if (request.Passengers.Count > 9)
            {
                _logger.LogWarning(
                    "Booking failed: passenger count {Count} exceeds limit for FlightId={FlightId}",
                    request.Passengers.Count, request.FlightId);
                throw new ValidationException("Maximum 9 passengers allowed per booking.");
            }

            ValidatePassengers(request, _logger);

            var flight = _flightRepository
                .GetAll()
                .FirstOrDefault(f => f.FlightId == request.FlightId);

            if (flight == null)
            {
                _logger.LogWarning("Booking failed: FlightId={FlightId} not found", request.FlightId);
                throw new NotFoundException("Flight not found.");
            }

            var cabin = flight.Cabins
                .FirstOrDefault(c => c.CabinClass == request.CabinClass);

            if (cabin == null)
            {
                _logger.LogWarning(
                    "Booking failed: CabinClass={CabinClass} not available on FlightId={FlightId}",
                    request.CabinClass, request.FlightId);
                throw new ValidationException(
                    $"Cabin class {request.CabinClass} not available on this flight.");
            }

            if (cabin.SeatsAvailable < request.NumberOfPassengers)
            {
                _logger.LogWarning(
                    "Booking failed: insufficient seats on FlightId={FlightId}, CabinClass={CabinClass}. " +
                    "Requested={Requested}, Available={Available}",
                    request.FlightId, request.CabinClass, request.NumberOfPassengers, cabin.SeatsAvailable);
                throw new SeatsUnavailableException("Insufficient seats available.");
            }

            cabin.SeatsAvailable -= request.NumberOfPassengers;

            decimal farePerPassenger =
                _fareCalculator.CalculateFinalFare(
                    flight.Airline,
                    flight.DistanceMiles,
                    request.CabinClass);

            decimal totalFare =
                farePerPassenger * request.NumberOfPassengers;

            var bookingReference = $"BK{Guid.NewGuid().ToString("N")[..8].ToUpper()}";

            _logger.LogInformation(
                "Booking confirmed: BookingReference={BookingReference}, FlightId={FlightId}, Airline={Airline}, " +
                "CabinClass={CabinClass}, PassengerCount={PassengerCount}, FarePerPassenger={FarePerPassenger}, TotalFare={TotalFare}",
                bookingReference, flight.FlightId, flight.Airline, request.CabinClass,
                request.NumberOfPassengers, farePerPassenger, totalFare);

            return new BookingResponse
            {
                BookingReference = bookingReference,

                FarePerPassenger = farePerPassenger,
                TotalFare = totalFare,
                PassengerCount = request.NumberOfPassengers,

                PassengerNames = request.Passengers
                    .Select(p => p.FullName)
                    .ToList()
            };
        }

        private static void ValidatePassengers(
            BookingRequest request, ILogger<BookingService> logger)
        {
            for (int i = 0; i < request.Passengers.Count; i++)
            {
                var passenger = request.Passengers[i];
                var passengerNumber = i + 1;

                if (string.IsNullOrWhiteSpace(passenger.FullName))
                {
                    logger.LogWarning(
                        "Booking failed: Passenger {PassengerNumber} missing full name (FlightId={FlightId})",
                        passengerNumber, request.FlightId);
                    throw new ValidationException(
                        $"Passenger {passengerNumber}: Full name is required.");
                }

                if (string.IsNullOrWhiteSpace(passenger.EmailAddress))
                {
                    logger.LogWarning(
                        "Booking failed: Passenger {PassengerNumber} missing email (FlightId={FlightId})",
                        passengerNumber, request.FlightId);
                    throw new ValidationException(
                        $"Passenger {passengerNumber}: Email address is required.");
                }

                if (string.IsNullOrWhiteSpace(passenger.DocumentNumber))
                {
                    logger.LogWarning(
                        "Booking failed: Passenger {PassengerNumber} missing document number (FlightId={FlightId})",
                        passengerNumber, request.FlightId);
                    throw new ValidationException(
                        $"Passenger {passengerNumber}: Document number is required.");
                }
            }
        }
    }
}