using Microsoft.Extensions.Logging;
using Server.Domain;

namespace Server.Services;

public sealed class ContractService : IContractService
{
    private readonly ILogger<ContractService> logger;

    public ContractService(ILogger<ContractService> logger) {
        this.logger = logger;
    }

    public DefaultResponse handle(Contract contract) {
        if (contract.Access != null) {
            foreach (AccessEntry entry in contract.Access) {
                long ts = entry.Timestamp;
                string id = entry.Id ?? "<null>";

                this.logger.LogInformation("Access entry => timestamp:{Timestamp} id:{Id}", ts, id);
            }
        } else {
            this.logger.LogInformation("No 'access' array provided.");
        }

        return new DefaultResponse(200, "Processed successfully");
    }
}
