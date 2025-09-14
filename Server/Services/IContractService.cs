using Server.Domain;

namespace Server.Services;

public interface IContractService
{
    DefaultResponse Handle(Contract contract);
}