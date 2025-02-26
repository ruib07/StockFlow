using StockFlow.Application.Interfaces;

namespace StockFlow.Application.Services;

public class AdministratorsService
{
    private readonly IAdministratorRepository _administratorRepository;

    public AdministratorsService(IAdministratorRepository administratorRepository)
    {
        _administratorRepository = administratorRepository;
    }
}
