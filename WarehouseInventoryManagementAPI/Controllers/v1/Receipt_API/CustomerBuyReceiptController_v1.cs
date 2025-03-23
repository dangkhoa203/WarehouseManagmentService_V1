using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseInventoryManagementAPI.Data;
using WarehouseInventoryManagementAPI.DTO.Receipt.Change;
using WarehouseInventoryManagementAPI.Model.Entity;
using WarehouseInventoryManagementAPI.Services.Product_Related_Service;
using WarehouseInventoryManagementAPI.Services.Receipt_Related_Service;

namespace WarehouseInventoryManagementAPI.Controllers.v1.Receipt_API
{
    [Route("api/v1/")]
    [ApiController]
    public class CustomerBuyReceiptController_v1 : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Account> _userManager;
        private readonly CustomerBuyReceiptService _customerBuyReceiptService;
        public CustomerBuyReceiptController_v1(ApplicationDbContext context, UserManager<Account> userManager, CustomerBuyReceiptService customerBuyReceiptService)
        {
            _context = context;
            _userManager = userManager;
            _customerBuyReceiptService = customerBuyReceiptService;
        }
        [HttpPost("Customer-Receipts"),Authorize]
        public async Task<IActionResult> NewReceipt(NewCustomerBuyReceiptDTO model)
        {
            var service = _context.Users.Include(u => u.ServiceRegistered).Where(u => u.UserName == User.Identity.Name).Select(u => u.ServiceRegistered).FirstOrDefault();
            if (await _customerBuyReceiptService.Add(model, service))
                return Ok("OK");
            else
                return BadRequest("Error");
        }
        [HttpDelete("Customer-Receipts/{id}"), Authorize]
        public async Task<IActionResult> DeleteReceipt(string id)
        {
            var service = _context.Users.Include(u => u.ServiceRegistered).Where(u => u.UserName == User.Identity.Name).Select(u => u.ServiceRegistered).FirstOrDefault();
            if (await _customerBuyReceiptService.Delete(id))
                return Ok("OK");
            else
                return BadRequest("Error");
        }
        [HttpPut("Customer-Receipts"), Authorize]
        public async Task<IActionResult> UpdateReceipt(NewCustomerBuyReceiptDTO model)
        {
            var service = _context.Users.Include(u => u.ServiceRegistered).Where(u => u.UserName == User.Identity.Name).Select(u => u.ServiceRegistered).FirstOrDefault();
            if (await _customerBuyReceiptService.Update(model))
                return Ok("OK");
            else
                return BadRequest("Error");
        }
        [HttpGet("Customer-Receipts"), Authorize]
        public async Task<IActionResult> Receipts()
        {
            var service = _context.Users.Include(u => u.ServiceRegistered).Where(u => u.UserName == User.Identity.Name).Select(u => u.ServiceRegistered).FirstOrDefault();
            try
            {
                return Ok(await _customerBuyReceiptService.GetAll(service));
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong!");
            }
        }
        [HttpGet("Customer-Receipts/{id}"), Authorize]
        public async Task<IActionResult> Receipt(string id)
        {
            var service = _context.Users.Include(u => u.ServiceRegistered).Where(u => u.UserName == User.Identity.Name).Select(u => u.ServiceRegistered).FirstOrDefault();
            try
            {
                var result=await _customerBuyReceiptService.FindById(service, id);
                if(result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong!");
            }
        }
        [HttpGet("Customer-Receipts/Export"), Authorize]
        public async Task<IActionResult> ReceiptsExport()
        {
            var service = _context.Users.Include(u => u.ServiceRegistered).Where(u => u.UserName == User.Identity.Name).Select(u => u.ServiceRegistered).FirstOrDefault();
            try
            {
                return Ok(await _customerBuyReceiptService.GetAll_ExportForm(service));
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong!");
            }
        }
        [HttpGet("Customer-Receipts/Return"), Authorize]
        public async Task<IActionResult> ReceiptsReturn()
        {
            var service = _context.Users.Include(u => u.ServiceRegistered).Where(u => u.UserName == User.Identity.Name).Select(u => u.ServiceRegistered).FirstOrDefault();
            try
            {
                return Ok(await _customerBuyReceiptService.GetAll_ReturnForm(service));
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong!");
            }
        }
    }
}
