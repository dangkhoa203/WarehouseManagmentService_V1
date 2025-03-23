using Microsoft.EntityFrameworkCore;
using WarehouseInventoryManagementAPI.DTO.Product.Change;
using WarehouseInventoryManagementAPI.DTO.Product.Show;
using WarehouseInventoryManagementAPI.Model.Entity;
using WarehouseInventoryManagementAPI.Model.Entity.Product_Entity;


using WarehouseInventoryManagementAPI.Repository;
using WarehouseInventoryManagementAPI.Services.Interface;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WarehouseInventoryManagementAPI.Services.Product_Related_Service
{
    public class ProductService : IDataService<Product, NewProductDTO, ShowProductDTO>
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductGroup> _productGroupRepository;
        public ProductService(IRepository<Product> productRepository, IRepository<ProductGroup> productGroupRepository)
        {
            _productGroupRepository = productGroupRepository;
            _productRepository = productRepository;
        }
        public async Task<bool> Add(NewProductDTO data, ServiceRegistered serviceRegistered)
        {
            Product product = new()
            {
                Name = data.Name,
                MeasureUnit = data.MeasureUnit,
                PricePerUnit = data.PricePerUnit,
                ProductGroup = await _productGroupRepository.GetById(data.GroupId),
                ServiceRegisteredFrom = serviceRegistered,
            };
            await _productRepository.Create(product);
            return await _productRepository.Save();
        }

        public async Task<bool> Delete(string id)
        {
            await _productRepository.Delete(id);
            return await _productRepository.Save();
        }

        public Task<List<ShowProductDTO>> FilterList(string query)
        {
            throw new NotImplementedException();
        }

        public async Task<ShowProductDTO> FindById(string id, ServiceRegistered serviceRegistered)
        {
            var product = await _productRepository.getFromDatabase()
                   .Include(p => p.ProductGroup)
                   .Where(p => !p.IsDeleted)
                   .Where(g => g.ServiceRegisteredFrom.Id == serviceRegistered.Id)
                   .FirstOrDefaultAsync(p=>p.Id==id);
            if(product != null)
            {
                return new ShowProductDTO()
                {
                    Id = product.Id,
                    Name = product.Name,
                    MeasureUnit = product.MeasureUnit,
                    PricePerUnit = product.PricePerUnit,
                    CreateDate = product.CreatedDate,
                    Group = product.ProductGroup != null ? new ShowProductGroupDTO()
                    {
                        Id=product.ProductGroup.Id,
                        Name = product.ProductGroup.Name,
                    } : null,
                };
            }
            return null;
        }

        public async Task<List<ShowProductDTO>> GetAll(ServiceRegistered service)
        {
            var products = await _productRepository.getFromDatabase()
                  .Include(p=>p.ProductGroup)
                  .Where(p=>!p.IsDeleted)
                  .Where(g => g.ServiceRegisteredFrom.Id == service.Id)
                  .OrderByDescending(p => p.CreatedDate)
                  .ToListAsync();
            var result = products
                .Select(product => new ShowProductDTO()
                {
                    Id=product.Id,
                    Name = product.Name,
                    MeasureUnit = product.MeasureUnit,
                    PricePerUnit = product.PricePerUnit,
                    CreateDate=product.CreatedDate,
                    Group = product.ProductGroup !=null ?  new ShowProductGroupDTO()
                    {
                        Name = product.ProductGroup.Name,
                        Description = product.ProductGroup.Description,
                    } : null,
                })
                .ToList();
            return result;
        }

        public async Task<bool> Update(NewProductDTO data)
        {
            Product product = new()
            {
                Id = data.Id,
                Name = data.Name,
                MeasureUnit = data.MeasureUnit,
                PricePerUnit = data.PricePerUnit,
                ProductGroup = await _productGroupRepository.GetById(data.GroupId),
            };
            await _productRepository.Update(product);
            return await _productRepository.Save();
        }
    }
}
