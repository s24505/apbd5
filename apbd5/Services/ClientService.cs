using Microsoft.AspNetCore.Mvc;

namespace apbd5.Services
{
    public class ClientService : IClientService
    {
        private readonly Context.Context _context;

        public ClientService(Context.Context context)
        {
            _context = context;
        }

        public IActionResult DeleteDataOfClient(int idClient)
        {
            if (ClientHasTrips(idClient))
            {
                return new BadRequestObjectResult("Wycieczka dla tego klienta istnieje");
            }

            var client = _context.Clients.SingleOrDefault(c => c.IdClient == idClient);
            if (client != null)
            {
                _context.Clients.Remove(client);
                _context.SaveChanges();
                return new OkObjectResult("Klient został usunięty");
            }

            return new NotFoundObjectResult("Klient nie został znaleziony");
        }

        private bool ClientHasTrips(int idClient)
        {
            return _context.ClientTrips.Any(x => x.IdClient == idClient);
        }
    }
}