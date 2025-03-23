using WarehouseInventoryManagementAPI.DTO.Form.Change;
using WarehouseInventoryManagementAPI.DTO.Form.Show;
using WarehouseInventoryManagementAPI.Model.Entity;
using WarehouseInventoryManagementAPI.Model.Form;
using WarehouseInventoryManagementAPI.Model.Receipt;
using WarehouseInventoryManagementAPI.Repository.Warehouse_Related_Repository;
using WarehouseInventoryManagementAPI.Repository;
using WarehouseInventoryManagementAPI.Services.Interface;
using WarehouseInventoryManagementAPI.Repository.Receipt_Related_Repository;
using WarehouseInventoryManagementAPI.DTO.Receipt.Show;
using Microsoft.EntityFrameworkCore;

namespace WarehouseInventoryManagementAPI.Services.Form_Related_Service
{
    public class ReturnReplenishFormService : IFormService<ReturnReplenishForm, NewFormDTO, ShowReturnReplenishForm>, IExportService<VendorReplenishReceipt>
    {
        private readonly IRepository<ReturnReplenishForm> _returnReplenishFormRepository;
        private readonly IRepository<VendorReplenishReceipt> _vendorReplenishReceiptRepository;
        private readonly IImportExportRepository _importExportRepository;
        public ReturnReplenishFormService(IRepository<ReturnReplenishForm> returnReplenishFormRepository, IRepository<VendorReplenishReceipt> vendorReplenishReceiptRepository, IImportExportRepository importExportRepository)
        {
            _returnReplenishFormRepository = returnReplenishFormRepository;
            _vendorReplenishReceiptRepository = vendorReplenishReceiptRepository;
            _importExportRepository = importExportRepository;
        }

        public async Task<bool> AddForm(NewFormDTO data, ServiceRegistered serviceRegistered)
        {
            var receipt = await _vendorReplenishReceiptRepository.GetById(data.ReceiptId);
            if (receipt == null || receipt.ReturnReplenishForm != null)
                return false;
            var form = new ReturnReplenishForm()
            {
                Receipt = receipt,
                OrderDate = data.OrderDate,
                ServiceRegisteredFrom = serviceRegistered,
            };
            await _returnReplenishFormRepository.Create(form);
            return await _returnReplenishFormRepository.Save();
        }

        public async Task<bool> DeleteForm(string id)
        {
            await _returnReplenishFormRepository.Delete(id);
            return await _returnReplenishFormRepository.Save();
        }

        public Task<List<ShowReturnReplenishForm>> FilterList(string query)
        {
            throw new NotImplementedException();
        }

        public async Task<ShowReturnReplenishForm> FindById(ServiceRegistered service, string id)
        {
            var Form = await _returnReplenishFormRepository.getFromDatabase()
              .Include(f => f.Receipt)
              .ThenInclude(r => r.Details)
              .ThenInclude(d => d.ProductNav)
              .Where(g => g.ServiceRegisteredFrom.Id == service.Id)
              .FirstOrDefaultAsync(f => f.Id == id);
            if (Form != null)
            {
                var result = new ShowReturnReplenishForm()
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

        public async Task<List<ShowReturnReplenishForm>> GetAll(ServiceRegistered service)
        {
            var Form = await _returnReplenishFormRepository.getFromDatabase()
                .Include(f => f.Receipt)
                .Where(g => g.ServiceRegisteredFrom.Id == service.Id)
                .OrderByDescending(f => f.CreatedDate)
                .ToListAsync();
            var result = Form
                .Select(f => new ShowReturnReplenishForm()
                {
                    Id = f.Id,
                    CreateDate = f.CreatedDate,
                    OrderDate = f.OrderDate,
                    Receipt = new ShowVendorReplenishReceiptDTO()
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
            var receipt = await _vendorReplenishReceiptRepository.GetById(data.ReceiptId);
            if (receipt == null || receipt.ReturnReplenishForm != null)
                return false;
            var form = new ReturnReplenishForm()
            {
                Receipt = receipt,
                OrderDate = data.OrderDate,
                ServiceRegisteredFrom = serviceRegistered,
            };
            await _returnReplenishFormRepository.Create(form);
            if (await CheckStock(receipt))
            {
                if (await Export(receipt, serviceRegistered))
                {
                    return await _returnReplenishFormRepository.Save();
                }
            }
            return false;
        }

        public async Task<bool> UpdateForm(NewFormDTO data)
        {
            var form = new ReturnReplenishForm()
            {
                Id = data.Id,
                OrderDate = data.OrderDate,
            };
            await _returnReplenishFormRepository.Update(form);
            return await _returnReplenishFormRepository.Save();
        }

        private async Task<bool> Export(VendorReplenishReceipt Receipt, ServiceRegistered service)
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

        private async Task<bool> CheckStock(VendorReplenishReceipt Receipt)
        {
            foreach (var item in Receipt.Details)
            {
                if (!await _importExportRepository.CheckQuantity(item.ProductId, item.Quantity))
                    return false;
            }
            return true;
        }

        public async Task<bool> CheckStock(NewFormDTO data)
        {
            var receipt = await _vendorReplenishReceiptRepository.GetById(data.ReceiptId);
            if (receipt == null || receipt.ReturnReplenishForm != null)
                return false;
            foreach (var item in receipt.Details)
            {
                if (!await _importExportRepository.CheckQuantity(item.ProductId, item.Quantity))
                    return false;
            }
            return true;
        }
    }
}
