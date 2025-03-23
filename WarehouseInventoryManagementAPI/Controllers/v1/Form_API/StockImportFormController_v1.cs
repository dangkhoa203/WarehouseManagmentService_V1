using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseInventoryManagementAPI.Data;
using WarehouseInventoryManagementAPI.DTO.Form.Change;
using WarehouseInventoryManagementAPI.Model.Entity;
using WarehouseInventoryManagementAPI.Services.Form_Related_Service;
using WarehouseInventoryManagementAPI.Services.Product_Related_Service;
using WarehouseInventoryManagementAPI.Services.Receipt_Related_Service;
using WarehouseInventoryManagementAPI.Services.Warehouse_Related_Service;

namespace WarehouseInventoryManagementAPI.Controllers.v1.Form_API
{
    [Route("api/v1/")]
    [ApiController]
    public class StockImportFormController_v1 : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Account> _userManager;
        private readonly StockImportFormService _stockImportFormService;
        public StockImportFormController_v1(ApplicationDbContext context, UserManager<Account> userManager, StockImportFormService stockImportFormService)
        {
            _context = context;
            _userManager = userManager;
            _stockImportFormService = stockImportFormService;

        }
        [HttpPost("Import-Forms"),Authorize]
        public async Task<IActionResult> AddForm(NewFormDTO model)
        {
            var service = _context.Users.Include(u => u.ServiceRegistered).Where(u => u.UserName == User.Identity.Name).Select(u => u.ServiceRegistered).FirstOrDefault();
            var result = model.UpdateStock.Value ? await _stockImportFormService.NewFormImport(model, service) : await _stockImportFormService.AddForm(model, service);
            if (result)
            {
                return Ok("OK");
            }
            else
                return BadRequest("Error");
        }
        [HttpDelete("Import-Forms/{id}"), Authorize]
        public async Task<IActionResult> DeleteForm([FromRoute] string id)
        {
            if (await _stockImportFormService.DeleteForm(id))
                return Ok("OK");
            else
                return BadRequest("Error");
        }
        [HttpPut("Import-Forms"), Authorize]
        public async Task<IActionResult> UpdateForm(NewFormDTO model)
        {
            if (await _stockImportFormService.UpdateForm(model))
                return Ok("OK");
            else
                return BadRequest("Error");
        }
        [HttpGet("Import-Forms"), Authorize]
        public async Task<IActionResult> Forms()
        {
            var service = _context.Users.Include(u => u.ServiceRegistered)
                                        .Where(u => u.UserName == User.Identity.Name)
                                        .Select(u => u.ServiceRegistered)
                                        .FirstOrDefault();
            try
            {
                return Ok(await _stockImportFormService.GetAll(service));
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong!");
            }
        }
        [HttpGet("Import-Forms/{id}"), Authorize]
        public async Task<IActionResult> Form(string id)
        {
            var service = _context.Users.Include(u => u.ServiceRegistered).Where(u => u.UserName == User.Identity.Name).Select(u => u.ServiceRegistered).FirstOrDefault();
            try
            {
                var result = await _stockImportFormService.FindById(service, id);
                if (result == null)
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
    }
}
