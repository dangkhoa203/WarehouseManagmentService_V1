using WarehouseInventoryManagementAPI.Data;

namespace WarehouseInventoryManagementAPI.Repository
{
    public interface IRepository<T> where T : class
    {
        
        IQueryable<T> getFromDatabase();
     
        Task Create(T item);

        Task Delete(string id);
        Task Update(T item);
        Task<bool> Save();
        Task<T> GetById(string id);
       
    }
}
