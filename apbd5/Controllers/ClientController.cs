using apbd5.Services;
using Microsoft.AspNetCore.Mvc;

namespace apbd5.Controllers
{
    [Route("api/clients")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpDelete("{idClient}")]
        public IActionResult DeleteClientData(int idClient)
        {
            var result = _clientService.DeleteDataOfClient(idClient);
            return Ok(result);
        }
    }
}