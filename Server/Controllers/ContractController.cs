using Microsoft.AspNetCore.Mvc;
using Server.Domain;
using Server.Services;

namespace Server.Controllers;

[ApiController]
[Route("api/v1/contracts")]
public sealed class ContractController(IContractService contractService) : ControllerBase
{
    [HttpPost("")]
    public DefaultResponse Handle([FromBody] Contract contract)
    {
        if (contract == null)
        {
            throw new ArgumentException("Contract cannot be null");
        }

        return contractService.Handle(contract);
    }
}