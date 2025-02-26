using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using StockFlow.Application.Services;
using System.Reflection;

namespace StockFlow.Application;

public static class ServiceRegistration
{
    public static void AddCustomServiceDependencyRegister(this IServiceCollection services, IConfiguration configuration)
    {
        var serviceAssemblies = new[]
        {
            typeof(AdministratorsService),
            typeof(CategoriesService),
            typeof(CustomersService),
            typeof(ProductsService),
            typeof(PurchaseItemsService),
            typeof(PurchasesService),
            typeof(SaleItemsService),
            typeof(SalesService),
            typeof(SuppliersService)
        };

        foreach (var serviceType in serviceAssemblies)
        {
            RegisterServicesFromAssembly(services, serviceType.Assembly);
        }
    }

    #region Private Methods

    private static void RegisterServicesFromAssembly(IServiceCollection services, Assembly assembly)
    {
        services.Scan(scan => scan.FromAssemblies(assembly)
                .AddClasses(classes => classes
                .Where(p => p.Name != null && p.Name.EndsWith("Service") && !p.IsInterface))
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsImplementedInterfaces()
                .WithScopedLifetime());
    }

    #endregion
}
