using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseInventoryManagementAPI.Data;
using WarehouseInventoryManagementAPI.DTO.Form.Change;
using WarehouseInventoryManagementAPI.Model.Entity;
using WarehouseInventoryManagementAPI.Services.Form_Related_Service;

namespace WarehouseInventoryManagementAPI.Controllers.v1.Form_API
{
    [Route("api/v1/")]
    [ApiController]
    public class StockExportFormController_v1 : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Account> _userManager;
        private readonly StockExportFormService _stockExportFormService;
        public StockExportFormController_v1(ApplicationDbContext context, UserManager<Account> userManager, StockExportFormService stockExportFormService)
        {
            _context = context;
            _userManager = userManager;
            _stockExportFormService = stockExportFormService;

        }
        [HttpPost("Export-Forms"), Authorize]
        public async Task<IActionResult> AddForm(NewFormDTO model)
        {
            var service = _context.Users.Include(u => u.ServiceRegistered).Where(u => u.UserName == User.Identity.Name).Select(u => u.ServiceRegistered).FirstOrDefault();
            var result = false;
            if (model.UpdateStock.Value)
            {
                if (!await _stockExportFormService.CheckStock(model))
                {
                    return BadRequest("Không đủ số lượng để xuất kho!");
                }
                result= await _stockExportFormService.NewFormExport(model, service);
            }
            else
            {
                result = await _stockExportFormService.AddForm(model, service);
            }
            if (result)
            {
                return Ok("OK");
            }
            else
                return BadRequest("Error");
        }
        [HttpDelete("Export-Forms/{id}"), Authorize]
        public async Task<IActionResult> DeleteForm([FromRoute] string id)
        {
            if (await _stockExportFormService.DeleteForm(id))
                return Ok("OK");
            else
                return BadRequest("Error");
        }
        [HttpPut("Export-Forms"), Authorize]
        public async Task<IActionResult> UpdateForm(NewFormDTO model)
        {
            if (await _stockExportFormService.UpdateForm(model))
                return Ok("OK");
            else
                return BadRequest("Error");
        }
        [HttpGet("Export-Forms"), Authorize]
        public async Task<IActionResult> Forms()
        {
            var service = _context.Users.Include(u => u.ServiceRegistered)
                                        .Where(u => u.UserName == User.Identity.Name)
                                        .Select(u => u.ServiceRegistered)
                                        .FirstOrDefault();
            try
            {
                return Ok(await _stockExportFormService.GetAll(service));
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong!");
            }
        }
        [HttpGet("Export-Forms/{id}"), Authorize]
        public async Task<IActionResult> Form(string id)
        {
            var service = _context.Users.Include(u => u.ServiceRegistered).Where(u => u.UserName == User.Identity.Name).Select(u => u.ServiceRegistered).FirstOrDefault();
            try
            {
                var result = await _stockExportFormService.FindById(service, id);
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
