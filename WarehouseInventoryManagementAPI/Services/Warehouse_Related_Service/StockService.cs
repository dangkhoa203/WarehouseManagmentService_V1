using Microsoft.EntityFrameworkCore;
using WarehouseInventoryManagementAPI.DTO.Customer.Show;
using WarehouseInventoryManagementAPI.DTO.Form.Change;
using WarehouseInventoryManagementAPI.DTO.Stock.Change;
using WarehouseInventoryManagementAPI.DTO.Stock.Show;
using WarehouseInventoryManagementAPI.Model.Entity;
using WarehouseInventoryManagementAPI.Model.Entity.Customer_Entity;
using WarehouseInventoryManagementAPI.Model.Form;
using WarehouseInventoryManagementAPI.Repository;
using WarehouseInventoryManagementAPI.Repository.Customer_Related_Repository;
using WarehouseInventoryManagementAPI.Repository.Receipt_Related_Repository;
using WarehouseInventoryManagementAPI.Services.Interface;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WarehouseInventoryManagementAPI.Services.Warehouse_Related_Service
{
    public class StockService : IStockService
    {
        private readonly IRepository<Stock> _stockRepository;
        private readonly IRepository<StockImportForm> _stockImportRepository;
        public StockService(IRepository<Stock> stockRepository)
        {
            _stockRepository = stockRepository;
        }
        public async Task<bool> Add(NewStockDTO data, ServiceRegistered serviceRegistered)
        {
            Stock stock = new()
            {
                ProductId = data.ProductId,
                Quantity = data.Quantity,
                ServiceRegisteredFrom=serviceRegistered
            };
            await _stockRepository.Create(stock);
            return await _stockRepository.Save();
        }

       
        public async Task<bool> Delete(string id)
        {
            await _stockRepository.Delete(id);
            return await _stockRepository.Save();
        }

        public Task<List<ShowStockDTO>> FilterList(string query)
        {
            throw new NotImplementedException();
        }

        public async Task<ShowStockDTO> FindById(string id)
        {
            var stock = await _stockRepository.GetById(id);
            if (stock == null)
            {
                return new ShowStockDTO()
                {
                    Id = stock.ProductNav.Id,
                    Name=stock.ProductNav.Name,
                    Quantity = stock.Quantity,
                };
            }
            return null;
        }

        public async Task<List<ShowStockDTO>> GetAll(ServiceRegistered service)
        {
            var stocks = await _stockRepository.getFromDatabase()
                   .Include(s => s.ProductNav)
                   .Where(g => g.ServiceRegisteredFrom.Id == service.Id)
                   .OrderByDescending(s => s.ProductId)
                   .ToListAsync();
            var result = stocks
                .Select(c => new ShowStockDTO()
                {
                    Id = c.ProductNav.Id,
                    Name = c.ProductNav.Name,
                    Quantity = c.Quantity,
                })
                .ToList();
            return result;
        }

        public async Task<bool> Update(NewStockDTO data)
        {
            Stock stock = new()
            {
                ProductId = data.ProductId,
                Quantity = data.Quantity,
            };
            await _stockRepository.Update(stock);
            return await _stockRepository.Save();
        }
    }
}
