using apbd5.ModelsDto;
using Microsoft.AspNetCore.Mvc;

namespace apbd5.Services
{
    public interface ITripService
    {
        List<TripDto> GetTripList();
        IActionResult AddClient(int idTrip, ClientTripDto addClientRequestDto);
    }
}