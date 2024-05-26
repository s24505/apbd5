using apbd5.Context;
using apbd5.Models;
using apbd5.ModelsDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apbd5.Services
{
    public class TripService : ITripService
    {
        private readonly Context.Context _dbContext;

        public TripService(Context.Context context)
        {
            _dbContext = context;
        }

        public List<TripDto> GetTripList()
        {
            return _dbContext.Trips
                .Include(t => t.IdCountries)
                .Include(t => t.ClientTrips)
                .OrderByDescending(t => t.DateFrom)
                .Select(t => new TripDto
                {
                    name = t.Name,
                    description = t.Description,
                    dateFrom = t.DateFrom,
                    dateTo = t.DateTo,
                    maxPeople = t.MaxPeople,
                    countries = t.IdCountries.Select(c => new CountryDto
                    {
                        name = c.Name
                    }).ToList(),
                    clients = t.ClientTrips.Select(ct => new ClientDto
                    {
                        firstName = ct.IdClientNavigation.FirstName,
                        lastName = ct.IdClientNavigation.LastName,
                    }).ToList()
                })
                .ToList();
        }

        public IActionResult AddClient(int idTrip, ClientTripDto clientTripDto)
        {
            var client = _dbContext.Clients.FirstOrDefault(c => c.Pesel == clientTripDto.Pesel);

            if (client == null)
            {
                client = new Client
                {
                    IdClient = _dbContext.Clients.Max(c => c.IdClient) + 1,
                    FirstName = clientTripDto.FirstName,
                    LastName = clientTripDto.LastName,
                    Email = clientTripDto.email,
                    Telephone = clientTripDto.Telephone,
                    Pesel = clientTripDto.Pesel
                };
                _dbContext.Clients.Add(client);
                _dbContext.SaveChanges();
            }

            var existingClientTrip = _dbContext.ClientTrips
                .Include(ct => ct.IdClientNavigation)
                .FirstOrDefault(ct => ct.IdTrip == idTrip && ct.IdClientNavigation.Pesel == clientTripDto.Pesel);

            if (existingClientTrip != null)
            {
                return new BadRequestObjectResult("Klient jest już zapisany wycieczkę");
            }

            var trip = _dbContext.Trips.FirstOrDefault(t => t.IdTrip == idTrip);
            if (trip == null)
            {
                return new BadRequestObjectResult("Wycieczka nie istnieje");
            }

            var clientId = _dbContext.Clients.FirstOrDefault(c => c.Pesel == clientTripDto.Pesel)?.IdClient;
            if (clientId == null)
            {
                return new BadRequestObjectResult("Brak klienta");
            }

            var newClientTrip = new ClientTrip
            {
                IdClient = clientId.Value,
                IdTrip = idTrip,
                RegisteredAt = DateTime.Now,
                PaymentDate = clientTripDto.PaymentDate
            };

            _dbContext.ClientTrips.Add(newClientTrip);
            _dbContext.SaveChanges();

            return new OkObjectResult("Klient został dodany do wycieczki");
        }
    }
}