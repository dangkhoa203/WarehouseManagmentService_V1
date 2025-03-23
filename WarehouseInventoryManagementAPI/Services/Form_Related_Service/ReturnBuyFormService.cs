using WarehouseInventoryManagementAPI.DTO.Form.Change;
using WarehouseInventoryManagementAPI.DTO.Form.Show;
using WarehouseInventoryManagementAPI.DTO.Receipt.Show;
using WarehouseInventoryManagementAPI.Model.Entity;
using WarehouseInventoryManagementAPI.Model.Form;
using WarehouseInventoryManagementAPI.Model.Receipt;
using WarehouseInventoryManagementAPI.Repository.Form_Related_Repository;
using WarehouseInventoryManagementAPI.Repository.Receipt_Related_Repository;
using WarehouseInventoryManagementAPI.Repository.Warehouse_Related_Repository;
using WarehouseInventoryManagementAPI.Repository;
using WarehouseInventoryManagementAPI.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace WarehouseInventoryManagementAPI.Services.Form_Related_Service
{
    public class ReturnBuyFormService : IFormService<ReturnBuyForm, NewFormDTO, ShowReturnBuyFormDTO>, IImportService<CustomerBuyReceipt>
    {
        private readonly IRepository<ReturnBuyForm> _returnBuyFormRepository;
        private readonly IRepository<CustomerBuyReceipt> _customerBuyReceiptRepository;
        private readonly IImportExportRepository _stockRepository;

        public ReturnBuyFormService(IRepository<ReturnBuyForm> returnBuyFormRepository, IRepository<CustomerBuyReceipt> customerBuyReceiptRepository, IImportExportRepository stockRepository)
        {
            _returnBuyFormRepository = returnBuyFormRepository;
            _customerBuyReceiptRepository = customerBuyReceiptRepository;
            _stockRepository = stockRepository;
        }
        public async Task<bool> AddForm(NewFormDTO data, ServiceRegistered serviceRegistered)
        {
            var receipt = await _customerBuyReceiptRepository.GetById(data.ReceiptId);
            if (receipt == null || receipt.ReturnBuyForm != null)
                return false;
            var form = new ReturnBuyForm()
            {
                Receipt = receipt,
                OrderDate = data.OrderDate,
                ServiceRegisteredFrom = serviceRegistered,
            };
            await _returnBuyFormRepository.Create(form);
            return await _returnBuyFormRepository.Save();
        }

        public async Task<bool> DeleteForm(string id)
        {
            await _returnBuyFormRepository.Delete(id);
            return await _returnBuyFormRepository.Save();
        }

        public Task<List<ShowReturnBuyFormDTO>> FilterList(string query)
        {
            throw new NotImplementedException();
        }

        public async Task<ShowReturnBuyFormDTO> FindById(ServiceRegistered service, string id)
        {
            var Form = await _returnBuyFormRepository.getFromDatabase()
              .Include(f => f.Receipt)
              .ThenInclude(r => r.Details)
              .ThenInclude(d => d.ProductNav)
              .Where(g => g.ServiceRegisteredFrom.Id == service.Id)
              .FirstOrDefaultAsync(f => f.Id == id);
            if (Form != null)
            {
                var result = new ShowReturnBuyFormDTO()
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
                            Quantity=d.Quantity
                        }).ToList(),
                    },
                };
                return result;
            }
            return null;
        }

        public async Task<List<ShowReturnBuyFormDTO>> GetAll(ServiceRegistered service)
        {
            var Form = await _returnBuyFormRepository.getFromDatabase()
                .Include(f => f.Receipt)
                .Where(g => g.ServiceRegisteredFrom.Id == service.Id)
                .OrderByDescending(f => f.CreatedDate)
                .ToListAsync();
            var result = Form
                .Select(f => new ShowReturnBuyFormDTO()
                {
                    Id = f.Id,
                    CreateDate = f.CreatedDate,
                    OrderDate = f.OrderDate,
                    Receipt = new ShowCustomerBuyReceiptDTO
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
            var receipt = await _customerBuyReceiptRepository.GetById(data.ReceiptId);
            if (receipt == null || receipt.ReturnBuyForm != null)
                return false;
            var form = new ReturnBuyForm()
            {
                Receipt = receipt,
                OrderDate = data.OrderDate,
                ServiceRegisteredFrom = serviceRegistered,
            };
            await _returnBuyFormRepository.Create(form);
            await Import(receipt, serviceRegistered);
            return await _returnBuyFormRepository.Save();
        }

        public async Task<bool> UpdateForm(NewFormDTO data)
        {
            var form = new ReturnBuyForm()
            {
                Id = data.Id,
                OrderDate = data.OrderDate,
            };
            await _returnBuyFormRepository.Update(form);
            return await _returnBuyFormRepository.Save();
        }

        private async Task Import(CustomerBuyReceipt receipt, ServiceRegistered service)
        {
            foreach (var item in receipt.Details)
            {
                Stock stock = new Stock()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    ServiceRegisteredFrom = service,
                };
                await _stockRepository.Add(stock);
            }
        }
    }
}
