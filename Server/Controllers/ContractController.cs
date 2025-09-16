using System;
using Microsoft.AspNetCore.Mvc;
using Server.Domain;
using Server.Services;

namespace Server.Controllers;

/// <summary>
/// API controller for managing <see cref="Contract"/> resources.
/// </summary>
[ApiController]
[Route("api/v1/contracts")]
public sealed class ContractController(IContractService contractService) : ControllerBase
{
    /// <summary>
    /// Handles a contract submitted via HTTP POST.
    /// </summary>
    /// <param name="contract">The <see cref="Contract"/> payload received in the request body.</param>
    /// <returns>
    /// A <see cref="DefaultResponse"/> containing the outcome of the operation, as defined by <see cref="IContractService.Handle(Contract)"/>.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown if the provided <paramref name="contract"/> is <c>null</c>.</exception>
    [HttpPost("")]
    public DefaultResponse Handle([FromBody] Contract contract)
    {
        return contract == null 
            ? throw new ArgumentException("Contract cannot be null") 
            : contractService.Handle(contract);
    }
}