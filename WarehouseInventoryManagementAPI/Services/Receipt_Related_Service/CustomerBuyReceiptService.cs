using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore;
using WarehouseInventoryManagementAPI.DTO.Customer.Show;
using WarehouseInventoryManagementAPI.DTO.Receipt.Change;
using WarehouseInventoryManagementAPI.DTO.Receipt.Show;
using WarehouseInventoryManagementAPI.DTO.Vendor.Show;
using WarehouseInventoryManagementAPI.Model.Entity;
using WarehouseInventoryManagementAPI.Model.Entity.Customer_Entity;
using WarehouseInventoryManagementAPI.Model.Entity.Product_Entity;
using WarehouseInventoryManagementAPI.Model.Receipt;
using WarehouseInventoryManagementAPI.Repository;
using WarehouseInventoryManagementAPI.Repository.Receipt_Related_Repository;
using WarehouseInventoryManagementAPI.Repository.Vendor_Related_Repository;
using WarehouseInventoryManagementAPI.Services.Interface;

namespace WarehouseInventoryManagementAPI.Services.Receipt_Related_Service
{
    public class CustomerBuyReceiptService : IReceiptService<CustomerBuyReceipt, NewCustomerBuyReceiptDTO, ShowCustomerBuyReceiptDTO>,IGetForExportService<ShowCustomerBuyReceiptDTO>
    {
        private readonly IRepository<CustomerBuyReceipt> _customerBuyReceiptRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Product> _productRepository;
        public CustomerBuyReceiptService(IRepository<CustomerBuyReceipt> customerBuyReceiptRepository, IRepository<Customer> customerRepository, IRepository<Product> productRepository)
        {
            _customerBuyReceiptRepository = customerBuyReceiptRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
        }
        public async Task<bool> Add(NewCustomerBuyReceiptDTO data, ServiceRegistered serviceRegistered)
        {
            var detail = new List<CustomerBuyReceiptDetail>();
            foreach(var d in data.Details)
            {
                var newdetail=new CustomerBuyReceiptDetail();
                newdetail.ProductNav = await _productRepository.GetById(d.ProductId);
                newdetail.Quantity= d.Quantity;
                detail.Add(newdetail);
            }
            var receipt=new CustomerBuyReceipt() {
                Customer=await _customerRepository.GetById(data.CustomerId),
                DateOrder=data.DateOrder,
                Details=detail,
                ServiceRegisteredFrom = serviceRegistered,
            };
            await _customerBuyReceiptRepository.Create(receipt);
            return await _customerRepository.Save();
        }

        public async Task<bool> Delete(string id)
        {
            await _customerBuyReceiptRepository.Delete(id);
            return await _productRepository.Save();
        }

        public Task<List<ShowCustomerBuyReceiptDTO>> FilterList(string query)
        {
            throw new NotImplementedException();
        }

        public async Task<ShowCustomerBuyReceiptDTO> FindById(ServiceRegistered service, string id)
        {
            var Receipt = await _customerBuyReceiptRepository.getFromDatabase()
                .Include(r => r.Details)
                .ThenInclude(d => d.ProductNav)
                .Include(r => r.Customer)
                .Where(g => g.ServiceRegisteredFrom.Id == service.Id)
                .Where(r=>!r.IsDeleted)
                .FirstOrDefaultAsync(r=>r.Id==id);
            if (Receipt == null)
            {
                return null;
            }
            var result = new ShowCustomerBuyReceiptDTO()
            {
                Id = Receipt.Id,
                CreateDate = Receipt.CreatedDate,
                DateOrder = Receipt.DateOrder,
                Customer = new DTO.Customer.Show.ShowCustomerDTO
                {
                    Id = Receipt.Customer.Id,
                    Name = Receipt.Customer.Name,
                    Email = Receipt.Customer.Email,
                    Address = Receipt.Customer.Address,
                    CreateDate = Receipt.Customer.CreatedDate,
                },
                Details = Receipt.Details.Select(d => new ShowDetailDTO()
                {
                    ProductId=d.ProductId,
                    ProductName = d.ProductNav.Name,
                    Quantity = d.Quantity,
                    TotalPrice = d.Quantity*d.ProductNav.PricePerUnit
                }).ToList(),
            };
            return result;
        }

        public async Task<List<ShowCustomerBuyReceiptDTO>> GetAll(ServiceRegistered service)
        {
            var Receipt = await _customerBuyReceiptRepository.getFromDatabase()
                .Include(r => r.Customer)
                .Include(r=>r.Details)
                .Where(g => g.ServiceRegisteredFrom.Id == service.Id)
                .OrderByDescending(r => r.CreatedDate)
                .ToListAsync();
            var result = Receipt.Where(g => !g.IsDeleted)
                .Select(g => new ShowCustomerBuyReceiptDTO()
                {
                    Id = g.Id,
                    CreateDate = g.CreatedDate,
                    DateOrder= g.DateOrder,
                    Customer = new DTO.Customer.Show.ShowCustomerDTO
                    {
                        Id = g.Customer.Id,
                        Name = g.Customer.Name,
                    },
                })
                .ToList();
            return result;
        }

        public async Task<List<ShowCustomerBuyReceiptDTO>> GetAll_ExportForm(ServiceRegistered service)
        {
            var Receipt = await _customerBuyReceiptRepository.getFromDatabase()
                 .Include(r=>r.StockExportReport)
                 .Where(r=>r.StockExportReport==null)
                 .Where(g => g.ServiceRegisteredFrom.Id == service.Id)
                 .OrderByDescending(r => r.DateOrder)
                 .ToListAsync();
            var result = Receipt.Where(g => !g.IsDeleted)
                .Select(g => new ShowCustomerBuyReceiptDTO()
                {
                    Id = g.Id,
                    DateOrder = g.DateOrder,
                })
                .ToList();
            return result;
        }

        public async Task<List<ShowCustomerBuyReceiptDTO>> GetAll_ReturnForm(ServiceRegistered service)
        {
            var Receipt = await _customerBuyReceiptRepository.getFromDatabase()
                .Include(r => r.ReturnBuyForm)
                .Where(r => r.ReturnBuyForm == null)
                .Where(g => g.ServiceRegisteredFrom.Id == service.Id)
                .OrderByDescending(r => r.DateOrder)
                .ToListAsync();
            var result = Receipt.Where(g => !g.IsDeleted)
                .Select(g => new ShowCustomerBuyReceiptDTO()
                {
                    Id = g.Id,
                    DateOrder = g.DateOrder,
                })
                .ToList();
            return result;
        }

        public async Task<bool> Update(NewCustomerBuyReceiptDTO data)
        {
            var detail = new List<CustomerBuyReceiptDetail>();
            foreach (var d in data.Details)
            {
                var newdetail = new CustomerBuyReceiptDetail();
                newdetail.ProductNav = await _productRepository.GetById(d.ProductId);
                newdetail.Quantity = d.Quantity;
                detail.Add(newdetail);
            }
            var receipt = new CustomerBuyReceipt()
            {
                Id=data.Id,
                Customer = await _customerRepository.GetById(data.CustomerId),
                DateOrder = data.DateOrder,
                Details = detail,
            };
            await _customerBuyReceiptRepository.Update(receipt);
            return await _customerBuyReceiptRepository.Save(); 
        }
    }
}
