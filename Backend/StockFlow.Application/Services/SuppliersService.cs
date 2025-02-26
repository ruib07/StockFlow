using StockFlow.Application.Interfaces;

namespace StockFlow.Application.Services;

public class SuppliersService
{
    private readonly ISupplierRepository _supplierRepository;
    
    public SuppliersService(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }
}
