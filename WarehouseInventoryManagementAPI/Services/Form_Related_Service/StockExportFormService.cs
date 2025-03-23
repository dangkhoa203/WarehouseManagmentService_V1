using Microsoft.EntityFrameworkCore;
using WarehouseInventoryManagementAPI.DTO.Form.Change;
using WarehouseInventoryManagementAPI.DTO.Form.Show;
using WarehouseInventoryManagementAPI.DTO.Receipt.Show;
using WarehouseInventoryManagementAPI.Model.Entity;
using WarehouseInventoryManagementAPI.Model.Form;
using WarehouseInventoryManagementAPI.Model.Receipt;
using WarehouseInventoryManagementAPI.Repository;
using WarehouseInventoryManagementAPI.Repository.Form_Related_Repository;
using WarehouseInventoryManagementAPI.Repository.Receipt_Related_Repository;
using WarehouseInventoryManagementAPI.Repository.Warehouse_Related_Repository;
using WarehouseInventoryManagementAPI.Services.Interface;

namespace WarehouseInventoryManagementAPI.Services.Form_Related_Service
{
    public class StockExportFormService : IFormService<StockExportForm, NewFormDTO, ShowExportFormDTO>, IExportService<CustomerBuyReceipt>
    {
        private readonly IRepository<StockExportForm> _stockExportFormRepository;
        private readonly IRepository<CustomerBuyReceipt> _customerBuyReceiptRepository;
        private readonly IImportExportRepository _importExportRepository;

        public StockExportFormService(IRepository<StockExportForm> stockExportFormRepository, IRepository<CustomerBuyReceipt> customerBuyReceiptRepository, IImportExportRepository importExportRepository)
        {
            _stockExportFormRepository = stockExportFormRepository;
            _customerBuyReceiptRepository= customerBuyReceiptRepository;
            _importExportRepository = importExportRepository;
        }
        public async Task<bool> AddForm(NewFormDTO data, ServiceRegistered serviceRegistered)
        {
            var receipt = await _customerBuyReceiptRepository.GetById(data.ReceiptId);
            if (receipt == null || receipt.StockExportReport != null)
                return false;
            var form = new StockExportForm()
            {
                Receipt = receipt,
                OrderDate = data.OrderDate,
                ServiceRegisteredFrom = serviceRegistered,
            };
            await _stockExportFormRepository.Create(form);
            return await _stockExportFormRepository.Save();
        }

        public async Task<bool> CheckStock(NewFormDTO data)
        {
            var receipt = await _customerBuyReceiptRepository.GetById(data.ReceiptId);
            if (receipt == null || receipt.StockExportReport != null)
                return false;
            foreach (var item in receipt.Details)
            {
                if (!await _importExportRepository.CheckQuantity(item.ProductId, item.Quantity))
                    return false;
            }
            return true;
        }

        public async Task<bool> DeleteForm(string id)
        {
            await _stockExportFormRepository.Delete(id);
            return await _stockExportFormRepository.Save();
        }

        public Task<List<ShowExportFormDTO>> FilterList(string query)
        {
            throw new NotImplementedException();
        }

        public async Task<ShowExportFormDTO> FindById(ServiceRegistered service, string id)
        {

            var Form = await _stockExportFormRepository.getFromDatabase()
              .Include(f => f.Receipt)
              .ThenInclude(r => r.Details)
              .ThenInclude(d => d.ProductNav)
              .Where(g => g.ServiceRegisteredFrom.Id == service.Id)
              .FirstOrDefaultAsync(f => f.Id == id);
            if (Form != null)
            {
                var result = new ShowExportFormDTO()
                {
                    Id = Form.Id,
                    CreateDate = Form.CreatedDate,
                    OrderDate = Form.OrderDate,
                    Receipt = new ShowCustomerBuyReceiptDTO
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

        public async Task<List<ShowExportFormDTO>> GetAll(ServiceRegistered service)
        {
            var Form = await _stockExportFormRepository.getFromDatabase()
                .Include(f => f.Receipt)
                .Where(g => g.ServiceRegisteredFrom.Id == service.Id)
                .OrderByDescending(f => f.CreatedDate)
                .ToListAsync();
            var result = Form
                .Select(f => new ShowExportFormDTO()
                {
                    Id = f.Id,
                    CreateDate = f.CreatedDate,
                    OrderDate = f.OrderDate,
                    Receipt = new ShowCustomerBuyReceiptDTO()
                    {
                        Id = f.Receipt.Id,
                        DateOrder = f.Receipt.DateOrder,
                    },
                })
                .ToList();
            return result;
        }

        public async Task<bool> NewFormExport(NewFormDTO data, ServiceRegistered serviceRegistered)
        {
            var receipt = await _customerBuyReceiptRepository.GetById(data.ReceiptId);
            if (receipt == null || receipt.StockExportReport != null)
                return false;
            var form = new StockExportForm()
            {
                Receipt = receipt,
                OrderDate = data.OrderDate,
                ServiceRegisteredFrom = serviceRegistered,
            };
            await _stockExportFormRepository.Create(form);
            if (await Export(receipt, serviceRegistered))
            {
                return await _stockExportFormRepository.Save();
            }
            return false;
        }

        public async Task<bool> UpdateForm(NewFormDTO data)
        {
            var form = new StockExportForm()
            {
                Id = data.Id,
                OrderDate = data.OrderDate,
            };
            await _stockExportFormRepository.Update(form);
            return await _stockExportFormRepository.Save();
        }

        private async Task<bool> CheckStock(CustomerBuyReceipt Receipt)
        {
            foreach (var item in Receipt.Details)
            {
                if (!await _importExportRepository.CheckQuantity(item.ProductId, item.Quantity))
                    return false;
            }
            return true;
        }

        private async Task<bool> Export(CustomerBuyReceipt Receipt, ServiceRegistered service)
        {
            foreach (var item in Receipt.Details)
            {
                Stock stock = new Stock()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                };
                if (!await _importExportRepository.Subtract(stock))
                {
                    return false;
                }
            }
            return true;
        }


    }
}
