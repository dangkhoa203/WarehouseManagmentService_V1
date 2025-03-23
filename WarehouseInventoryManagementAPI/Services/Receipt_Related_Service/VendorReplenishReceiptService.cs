using Microsoft.EntityFrameworkCore;
using WarehouseInventoryManagementAPI.DTO.Receipt.Change;
using WarehouseInventoryManagementAPI.DTO.Receipt.Show;
using WarehouseInventoryManagementAPI.DTO.Vendor.Show;
using WarehouseInventoryManagementAPI.Model.Entity;
using WarehouseInventoryManagementAPI.Model.Entity.Customer_Entity;
using WarehouseInventoryManagementAPI.Model.Entity.Product_Entity;
using WarehouseInventoryManagementAPI.Model.Entity.Vendor_EntiTy;
using WarehouseInventoryManagementAPI.Model.Receipt;
using WarehouseInventoryManagementAPI.Repository;
using WarehouseInventoryManagementAPI.Services.Interface;

namespace WarehouseInventoryManagementAPI.Services.Receipt_Related_Service
{
    public class VendorReplenishReceiptService : IReceiptService<VendorReplenishReceipt, NewVendorReplenishReceiptDTO, ShowVendorReplenishReceiptDTO>,IGetForImportService<ShowVendorReplenishReceiptDTO>
    {
        private readonly IRepository<VendorReplenishReceipt> _vendorReplenishReceiptRepository;
        private readonly IRepository<Vendor> _vendorRepository;
        private readonly IRepository<Product> _productRepository;
        public VendorReplenishReceiptService(IRepository<VendorReplenishReceipt> vendorReplenishReceiptRepository, IRepository<Vendor> vendorRepository, IRepository<Product> productRepository)
        {
            _vendorReplenishReceiptRepository = vendorReplenishReceiptRepository;
            _vendorRepository = vendorRepository;
            _productRepository = productRepository;
        }
        public async Task<bool> Add(NewVendorReplenishReceiptDTO data, ServiceRegistered serviceRegistered)
        {
            var detail = new List<VendorReplenishReceiptDetail>();
            foreach (var d in data.Details)
            {
                var newdetail = new VendorReplenishReceiptDetail();
                newdetail.ProductNav = await _productRepository.GetById(d.ProductId);
                newdetail.Quantity = d.Quantity;
                detail.Add(newdetail);
            }
            var receipt = new VendorReplenishReceipt()
            {
                Vendor = await _vendorRepository.GetById(data.VendorId),
                DateOrder = data.DateOrder,
                Details = detail,
                ServiceRegisteredFrom = serviceRegistered,
            };
            await _vendorReplenishReceiptRepository.Create(receipt);
            return await _vendorReplenishReceiptRepository.Save();
        }

        public async Task<bool> Delete(string id)
        {
            await _vendorReplenishReceiptRepository.Delete(id);
            return await _vendorReplenishReceiptRepository.Save();
        }

        public Task<List<ShowVendorReplenishReceiptDTO>> FilterList(string query)
        {
            throw new NotImplementedException();
        }

        public async Task<ShowVendorReplenishReceiptDTO> FindById(ServiceRegistered service, string id)
        {
            var Receipt = await _vendorReplenishReceiptRepository.getFromDatabase()
                .Include(r => r.Details)
                .ThenInclude(d => d.ProductNav)
                .Include(r => r.Vendor)
                .Where(g => g.ServiceRegisteredFrom.Id == service.Id)
                .Where(r => !r.IsDeleted)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (Receipt == null)
            {
                return null;
            }
            var result = new ShowVendorReplenishReceiptDTO()
            {
                Id = Receipt.Id,
                CreateDate = Receipt.CreatedDate,
                DateOrder = Receipt.DateOrder,
                Vendor = new ShowVendorDTO
                {
                    Id = Receipt.Vendor.Id,
                    Name = Receipt.Vendor.Name,
                    Email = Receipt.Vendor.Email,
                    Address = Receipt.Vendor.Address,
                    CreateDate = Receipt.Vendor.CreatedDate,
                },
                Details = Receipt.Details.Select(d => new ShowDetailDTO()
                {
                    ProductId = d.ProductId,
                    ProductName = d.ProductNav.Name,
                    Quantity = d.Quantity,
                    TotalPrice = d.Quantity * d.ProductNav.PricePerUnit
                }).ToList(),
            };
            return result;
        }

        public async Task<List<ShowVendorReplenishReceiptDTO>> GetAll(ServiceRegistered service)
        {
            var Receipt = await _vendorReplenishReceiptRepository.getFromDatabase()
                .Include(r => r.Vendor)
                .Include(r => r.Details)
                .Where(g => g.ServiceRegisteredFrom.Id == service.Id)
                .OrderByDescending(r=>r.CreatedDate)
                .ToListAsync();
            var result = Receipt.Where(g => !g.IsDeleted)
                .Select(g => new ShowVendorReplenishReceiptDTO()
                {
                    Id = g.Id,
                    CreateDate = g.CreatedDate,
                    DateOrder = g.DateOrder,
                    Vendor = new ShowVendorDTO
                    {
                        Id = g.Vendor.Id,
                        Name = g.Vendor.Name,
                    },
                })
                .ToList();
            return result;
        }

        public async Task<List<ShowVendorReplenishReceiptDTO>> GetAll_ImportForm(ServiceRegistered service)
        {
            var Receipt = await _vendorReplenishReceiptRepository.getFromDatabase()
               .Include(r=>r.StockImportReport)
               .Where(r=>r.StockImportReport==null)
               .Where(g => g.ServiceRegisteredFrom.Id == service.Id)
               .OrderByDescending(r => r.DateOrder)
               .ToListAsync();
            var result = Receipt.Where(g => !g.IsDeleted)
                .Select(g => new ShowVendorReplenishReceiptDTO()
                {
                    Id = g.Id,
                    DateOrder = g.DateOrder,
                })
                .ToList();
            return result;
        }

        public async Task<List<ShowVendorReplenishReceiptDTO>> GetAll_ReturnForm(ServiceRegistered service)
        {
            var Receipt = await _vendorReplenishReceiptRepository.getFromDatabase()
               .Include(r => r.ReturnReplenishForm)
               .Where(r => r.ReturnReplenishForm == null)
               .Where(g => g.ServiceRegisteredFrom.Id == service.Id)
               .OrderByDescending(r => r.DateOrder)
               .ToListAsync();
            var result = Receipt.Where(g => !g.IsDeleted)
                .Select(g => new ShowVendorReplenishReceiptDTO()
                {
                    Id = g.Id,
                    DateOrder = g.DateOrder,
                })
                .ToList();
            return result;
        }

        public async Task<bool> Update(NewVendorReplenishReceiptDTO data)
        {
            var detail = new List<VendorReplenishReceiptDetail>();
            foreach (var d in data.Details)
            {
                var newdetail = new VendorReplenishReceiptDetail();
                newdetail.ProductNav = await _productRepository.GetById(d.ProductId);
                newdetail.Quantity = d.Quantity;
                detail.Add(newdetail);
            }
            var receipt = new VendorReplenishReceipt()
            {
                Id = data.Id,
                Vendor = await _vendorRepository.GetById(data.VendorId),
                DateOrder = data.DateOrder,
                Details = detail,
            };
            await _vendorReplenishReceiptRepository.Update(receipt);
            return await _vendorReplenishReceiptRepository.Save();
        }
    }
}
