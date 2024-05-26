using apbd5.ModelsDto;
using apbd5.Services;
using Microsoft.AspNetCore.Mvc;

namespace apbd5.Controllers
{
    [Route("api/trips")]
    [ApiController]
    public class TripController : ControllerBase
    {
        private readonly ITripService _tripService;

        public TripController(ITripService tripService)
        {
            _tripService = tripService;
        }

        [HttpGet]
        public IActionResult GetTrips()
        {
            var result = _tripService.GetTripList();
            return Ok(result);
        }

        [HttpPost("{idTrip}/clients")]
        public IActionResult AddClientToTrip(int idTrip, [FromBody] ClientTripDto addClientRequestDto)
        {
            var result = _tripService.AddClient(idTrip, addClientRequestDto);
            return Ok(result);
        }
    }
}