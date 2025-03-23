using WarehouseInventoryManagementAPI.Model.Entity.Customer_Entity;
using WarehouseInventoryManagementAPI.Repository.Customer_Related_Repository;
using WarehouseInventoryManagementAPI.Repository;
using WarehouseInventoryManagementAPI.Services.Customer_Related_Service;
using WarehouseInventoryManagementAPI.Services.Interface;
using WarehouseInventoryManagementAPI.DTO.Customer.Change;
using WarehouseInventoryManagementAPI.DTO.Customer.Show;
using WarehouseInventoryManagementAPI.Model.Entity.Product_Entity;
using WarehouseInventoryManagementAPI.Repository.Product_Related_Repository;
using WarehouseInventoryManagementAPI.Model.Entity.Vendor_EntiTy;
using WarehouseInventoryManagementAPI.Repository.Vendor_Related_Repository;
using WarehouseInventoryManagementAPI.Model.Entity.Vendor_Entity;
using WarehouseInventoryManagementAPI.Services.Product_Related_Service;
using WarehouseInventoryManagementAPI.Services.Vendor_Related_Service;
using WarehouseInventoryManagementAPI.Model.Receipt;
using WarehouseInventoryManagementAPI.Repository.Receipt_Related_Repository;
using WarehouseInventoryManagementAPI.Services.Receipt_Related_Service;
using WarehouseInventoryManagementAPI.Model.Form;
using WarehouseInventoryManagementAPI.Repository.Form_Related_Repository;
using WarehouseInventoryManagementAPI.Services.Form_Related_Service;
using WarehouseInventoryManagementAPI.Model.Entity;
using WarehouseInventoryManagementAPI.Repository.Warehouse_Related_Repository;
using WarehouseInventoryManagementAPI.Services.Warehouse_Related_Service;
namespace WarehouseInventoryManagementAPI
{
    public static class ServicesExtensions
    {
        public static void AddAllRepository(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Customer>, CustomerRepository>();
            services.AddScoped<IRepository<CustomerGroup>,CustomerGroupRepository>();
            services.AddScoped<IRepository<Product>, ProductRepository>();
            services.AddScoped<IRepository<ProductGroup>, ProductGroupRepository>();
            services.AddScoped<IRepository<Vendor>, VendorRepository>();
            services.AddScoped<IRepository<VendorGroup>, VendorGroupRepository>();
            services.AddScoped<IRepository<CustomerBuyReceipt>, CustomerBuyReceiptRepository>();
            services.AddScoped<IRepository<VendorReplenishReceipt>, VendorReplenishReceiptRepository>();
            services.AddScoped<IRepository<StockImportForm>, StockImportFormRepository>();
            services.AddScoped<IRepository<StockExportForm>, StockExportFormRepository>();
            services.AddScoped<IRepository<ReturnBuyForm>, ReturnBuyFormRepository>();
            services.AddScoped<IRepository<ReturnReplenishForm>, ReturnReplenishFormRepository>();
            services.AddScoped<IRepository<Stock>,StockRepository>();
            services.AddScoped<IImportExportRepository, StockRepository>();
        }
        public static void AddAllService(this IServiceCollection services)
        {
            services.AddScoped<CustomerService>();
            services.AddScoped<CustomerGroupService>();
            services.AddScoped<CustomerBuyReceiptService>();
            services.AddScoped<ProductService>();
            services.AddScoped<ProductGroupService>();
            services.AddScoped<VendorService>();
            services.AddScoped<VendorGroupService>();
            services.AddScoped<VendorReplenishReceiptService>();
            services.AddScoped<StockImportFormService>();
            services.AddScoped<StockExportFormService>();
            services.AddScoped<StockService>();
            services.AddScoped<ReturnBuyFormService>();
            services.AddScoped<ReturnReplenishFormService>();
        }
    }
}
