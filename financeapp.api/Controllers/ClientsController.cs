namespace FinanceApp.Api;
using FinanceApp.Data;
using FinanceApp.Domain;
using FinanceApp.Services;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ClientsController(ILogger<ClientsController> logger, IClientRepository clientRepository) : ControllerBase
{
    private readonly ILogger<ClientsController> logger = logger;
    private readonly IClientRepository clientRepository = clientRepository;

    [HttpGet]
    [Route("search")]
    public async Task<IActionResult> GetQuotes([FromQuery] string search = "")
    {
        var clients = await clientRepository.Search(search);

        return Ok(clients);
    }
}