using StockFlow.Application.Helpers;
using StockFlow.Application.Interfaces;
using StockFlow.Domain.Entities;

namespace StockFlow.Application.Services;

public class SuppliersService
{
    private readonly ISupplierRepository _supplierRepository;
    
    public SuppliersService(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public async Task<IEnumerable<Suppliers>> GetSuppliers()
    {
        return await _supplierRepository.GetSuppliers();
    }

    public async Task<Suppliers> GetSupplierById(Guid supplierId)
    {
        var supplier = await _supplierRepository.GetSupplierById(supplierId);

        if (supplier == null) ErrorHelper.ThrowNotFoundException("Supplier not found!");

        return supplier;
    }

    public async Task<Suppliers> CreateSupplier(Suppliers supplier)
    {
        var existingSupplier = await _supplierRepository.GetSupplierByEmail(supplier.Email);

        if (existingSupplier != null) ErrorHelper.ThrowConflictException("Email already in use!");

        return await _supplierRepository.CreateSupplier(supplier);
    }

    public async Task<Suppliers> UpdateSupplier(Guid supplierId, Suppliers updateSupplier)
    {
        var currentSupplier = await GetSupplierById(supplierId);
        var emailExists = await _supplierRepository.GetSupplierByEmail(updateSupplier.Email);

        if (emailExists != null && emailExists.Id != supplierId) ErrorHelper.ThrowConflictException("Email already in use!");

        currentSupplier.Name = updateSupplier.Name;
        currentSupplier.NIF = updateSupplier.NIF;
        currentSupplier.PhoneNumber = updateSupplier.PhoneNumber;
        currentSupplier.Email = updateSupplier.Email;
        currentSupplier.Address = updateSupplier.Address;

        await _supplierRepository.UpdateSupplier(currentSupplier);
        return currentSupplier;
    }

    public async Task DeleteSupplier(Guid supplierId)
    {
        await _supplierRepository.DeleteSupplier(supplierId);
    }
}
