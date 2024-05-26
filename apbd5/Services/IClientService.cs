using Microsoft.AspNetCore.Mvc;

namespace apbd5.Services
{
    public interface IClientService
    {
        IActionResult DeleteDataOfClient(int idClient);
    }
}