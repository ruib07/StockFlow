using StockFlow.Domain.Entities;

namespace StockFlow.Application.Interfaces;

public interface ISupplierRepository
{
    Task<IEnumerable<Suppliers>> GetSuppliers();
    Task<Suppliers> GetSupplierById(Guid supplierId);
    Task<Suppliers> CreateSupplier(Suppliers supplier);
    Task UpdateSupplier(Suppliers supplier);
    Task DeleteSupplier(Guid supplierId);
}
