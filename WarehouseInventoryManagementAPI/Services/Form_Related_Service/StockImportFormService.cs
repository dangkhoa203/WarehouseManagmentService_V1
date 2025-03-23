using Microsoft.EntityFrameworkCore;
using WarehouseInventoryManagementAPI.DTO.Form.Change;
using WarehouseInventoryManagementAPI.DTO.Form.Show;
using WarehouseInventoryManagementAPI.DTO.Receipt.Show;
using WarehouseInventoryManagementAPI.DTO.Vendor.Show;
using WarehouseInventoryManagementAPI.Model.Entity;
using WarehouseInventoryManagementAPI.Model.Entity.Customer_Entity;
using WarehouseInventoryManagementAPI.Model.Form;
using WarehouseInventoryManagementAPI.Model.Receipt;
using WarehouseInventoryManagementAPI.Repository;
using WarehouseInventoryManagementAPI.Repository.Customer_Related_Repository;
using WarehouseInventoryManagementAPI.Repository.Warehouse_Related_Repository;
using WarehouseInventoryManagementAPI.Services.Interface;

namespace WarehouseInventoryManagementAPI.Services.Form_Related_Service
{
    public class StockImportFormService : IFormService<StockImportForm, NewFormDTO, ShowImportFormDTO>, IImportService<VendorReplenishReceipt>
    {
        private readonly IRepository<StockImportForm> _stockImportFormRepository;
        private readonly IRepository<VendorReplenishReceipt> _vendorReplenishReceiptRepository;
        private readonly IImportExportRepository _importExportRepository;

        public StockImportFormService(IRepository<StockImportForm> stockImportFormRepository, IRepository<VendorReplenishReceipt> vendorReplenishReceiptRepository, IImportExportRepository importExportRepository)
        {
            _stockImportFormRepository = stockImportFormRepository;
            _vendorReplenishReceiptRepository = vendorReplenishReceiptRepository;
            _importExportRepository = importExportRepository;
        }
        public async Task<bool> AddForm(NewFormDTO data, ServiceRegistered serviceRegistered)
        {
            var receipt = await _vendorReplenishReceiptRepository.GetById(data.ReceiptId);
            if (receipt == null || receipt.StockImportReport!=null)
                return false;
            var form = new StockImportForm()
            {
                Receipt = receipt,
                OrderDate = data.OrderDate,
                ServiceRegisteredFrom = serviceRegistered,
            };
            await _stockImportFormRepository.Create(form);
            return await _stockImportFormRepository.Save();
        }
        public async Task<bool> DeleteForm(string id)
        {
            await _stockImportFormRepository.Delete(id);
            return await _stockImportFormRepository.Save();
        }
        public Task<List<ShowImportFormDTO>> FilterList(string query)
        {
            throw new NotImplementedException();
        }
        public async Task<ShowImportFormDTO> FindById(ServiceRegistered service, string id)
        {
            var Form = await _stockImportFormRepository.getFromDatabase()
              .Include(f => f.Receipt)
              .ThenInclude(r=>r.Details)
              .ThenInclude(d=>d.ProductNav)
              .Where(g => g.ServiceRegisteredFrom.Id == service.Id)
              .FirstOrDefaultAsync(f => f.Id == id);
            if (Form != null)
            {
                var result = new ShowImportFormDTO()
                {
                    Id = Form.Id,
                    CreateDate = Form.CreatedDate,
                    OrderDate = Form.OrderDate,
                    Receipt = new ShowVendorReplenishReceiptDTO
                    {
                        Id = Form.Receipt.Id,
                        DateOrder = Form.Receipt.DateOrder,
                        Details = Form.Receipt.Details.Select(d => new ShowDetailDTO()
                        {
                            ProductId = d.ProductId,
                            ProductName = d.ProductNav.Name,
                            Quantity = d.Quantity,
                        }).ToList(),
                    },
                };
                return result;
            }
            return null;
        }
        public async Task<List<ShowImportFormDTO>> GetAll(ServiceRegistered service)
        {
            var Form = await _stockImportFormRepository.getFromDatabase()
                .Include(f => f.Receipt)
                .Where(g => g.ServiceRegisteredFrom.Id == service.Id)
                .OrderByDescending(f => f.CreatedDate)
                .ToListAsync();
            var result = Form
                .Select(f => new ShowImportFormDTO()
                {
                    Id = f.Id,
                    CreateDate = f.CreatedDate,
                    OrderDate = f.OrderDate,
                    Receipt = new ShowVendorReplenishReceiptDTO
                    {
                        Id = f.Receipt.Id,
                        DateOrder = f.Receipt.DateOrder,
                    },
                })
                .ToList();
            return result;
        }
        public async Task<bool> NewFormImport(NewFormDTO data, ServiceRegistered serviceRegistered)
        {
            var receipt = await _vendorReplenishReceiptRepository.GetById(data.ReceiptId);
            if (receipt == null || receipt.StockImportReport != null)
                return false;
            var form = new StockImportForm()
            {
                Receipt = receipt,
                OrderDate = data.OrderDate,
                ServiceRegisteredFrom = serviceRegistered,
            };
            await _stockImportFormRepository.Create(form);
            await Import(receipt, serviceRegistered);
            return await _stockImportFormRepository.Save();
        }
        public async Task<bool> UpdateForm(NewFormDTO data)
        {
            var form = new StockImportForm()
            {
                Id=data.Id,
                OrderDate = data.OrderDate,
            };
            await _stockImportFormRepository.Update(form);
            return await _stockImportFormRepository.Save();
        }
        private async Task Import(VendorReplenishReceipt receipt, ServiceRegistered service)
        {
            foreach (var item in receipt.Details)
            {
                Stock stock = new Stock()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    ServiceRegisteredFrom = service,
                };
                await _importExportRepository.Add(stock);
            }
        }
    }
}
