using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseInventoryManagementAPI.Data;
using WarehouseInventoryManagementAPI.DTO.Stock.Change;
using WarehouseInventoryManagementAPI.Model.Entity;
using WarehouseInventoryManagementAPI.Services.Receipt_Related_Service;
using WarehouseInventoryManagementAPI.Services.Warehouse_Related_Service;

namespace WarehouseInventoryManagementAPI.Controllers.v1.Stock_API
{
    [Route("api/v1/")]
    [ApiController]
    public class StockController_v1 : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Account> _userManager;
        private readonly StockService _stocktService;
        public StockController_v1(ApplicationDbContext context, UserManager<Account> userManager, StockService stocktService)
        {
            _context= context;
            _userManager= userManager;
            _stocktService= stocktService;
        }
        [HttpPost("stocks"),Authorize]
        public async Task<IActionResult> AddStock(NewStockDTO model)
        {
            var service = _context.Users.Include(u => u.ServiceRegistered).Where(u => u.UserName == User.Identity.Name).Select(u => u.ServiceRegistered).FirstOrDefault();
            if (await _stocktService.Add(model, service))
                return Ok("OK");
            else
                return BadRequest("Error");
        }
        [HttpDelete("stocks/{id}"),Authorize]
        public async Task<IActionResult> RemoveStock(string id)
        {
            if (await _stocktService.Delete(id))
                return Ok("OK");
            else
                return BadRequest("Error");
        }
        [HttpPut("stocks"), Authorize]
        public async Task<IActionResult> UpdateStock(NewStockDTO model)
        {
            if (await _stocktService.Update(model))
                return Ok("OK");
            else
                return BadRequest("Error");
        }
        [HttpGet("stocks"), Authorize]
        public async Task<IActionResult> Stocks()
        {
            var service = _context.Users.Include(u => u.ServiceRegistered).Where(u => u.UserName == User.Identity.Name).Select(u => u.ServiceRegistered).FirstOrDefault();
            try
            {
                return Ok(await _stocktService.GetAll(service));
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong!");
            }
        }
    }
}
