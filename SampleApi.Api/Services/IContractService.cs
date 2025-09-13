using Server.Domain;

namespace Server.Services;

public interface IContractService {
    DefaultResponse handle(Contract contract);
}
