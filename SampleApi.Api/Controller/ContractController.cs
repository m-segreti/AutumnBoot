using Microsoft.AspNetCore.Mvc;
using Server.Domain;
using Server.Services;

namespace Server.Controllers;

[ApiController]
[Route("api/v1/contracts")]
public sealed class ContractController : ControllerBase {
    private readonly IContractService contractService;

    public ContractController(IContractService contractService) {
        this.contractService = contractService;
    }

    [HttpPost("")]
    public DefaultResponse handle([FromBody] Contract contract) {
        if (contract == null) {
            throw new ArgumentException("Contract cannot be null");
        }

        return this.contractService.handle(contract);
    }
}
