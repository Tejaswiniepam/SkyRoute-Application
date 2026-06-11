using Microsoft.AspNetCore.Mvc;
using SkyRoute.Models;
using SkyRoute.Services;

namespace SkyRoute.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookFlights : ControllerBase
    {
        
        private readonly IBookingService _bookingService;

        public BookFlights(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost]
        public IActionResult Book([FromBody] BookingRequest request)
        {
            try
            {
                if (request == null)
                    return BadRequest("Booking request cannot be null.");

                var response = _bookingService.BookFlight(request);
                return Ok(new
                {
                    success = true,
                    bookingReference = response.BookingReference,
                    farePerPassenger = response.FarePerPassenger,
                    totalFare = response.TotalFare,
                    passengerCount = response.PassengerCount,
                    passengerNames = response.PassengerNames
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }
    }
}
