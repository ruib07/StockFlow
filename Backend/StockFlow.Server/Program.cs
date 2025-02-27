using StockFlow.Server.Configurations;
using StockFlow.Application;
using StockFlow.Server.Middlewares;
using StockFlow.Application.Constants;
using StockFlow.Application.Interfaces;
using StockFlow.Infrastructure.Repositories;
using StockFlow.Application.Services.Email;
using StockFlow.Application.Services;
using StockFlow.Application.Services.Email.Interfaces;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddCustomApiSecurity(configuration);
builder.Services.AddCustomServiceDependencyRegister(configuration);
builder.Services.AddCustomDatabaseConfiguration(configuration);

builder.Services.AddScoped<IAdministratorRepository, AdministratorRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IPurchaseItemRepository, PurchaseItemRepository>();
builder.Services.AddScoped<IPurchaseRepository, PurchaseRepository>();
builder.Services.AddScoped<ISaleItemRepository, SaleItemRepository>();
builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();
builder.Services.AddScoped<IEmailPasswordReset, EmailPasswordResetService>();

builder.Services.AddScoped<AdministratorsService>();
builder.Services.AddScoped<CategoriesService>();
builder.Services.AddScoped<CustomersService>();
builder.Services.AddScoped<ProductsService>();
builder.Services.AddScoped<PurchaseItemsService>();
builder.Services.AddScoped<PurchasesService>();
builder.Services.AddScoped<SaleItemsService>();
builder.Services.AddScoped<SalesService>();
builder.Services.AddScoped<SuppliersService>();

builder.Services.AddAuthorizationBuilder()
                .AddPolicy(AppSettings.PolicyAdmin, policy => policy
                .RequireRole(AppSettings.PolicyAdmin));

builder.Services.AddCors(options =>
{
    options.AddPolicy(AppSettings.AllowLocalhost,
        builder =>
        {
            builder.WithOrigins(AppSettings.OriginReact)
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
        });
});

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseCors(AppSettings.AllowLocalhost);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
