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
    [Route("api/v1")]
    [ApiController]
    public class ReturnReplenishFormController_v1 : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Account> _userManager;
        private readonly ReturnReplenishFormService _returnReplenishFormService;
        public ReturnReplenishFormController_v1(ApplicationDbContext context, UserManager<Account> userManager, ReturnReplenishFormService returnReplenishFormService)
        {
            _context = context;
            _userManager = userManager;
            _returnReplenishFormService = returnReplenishFormService;

        }
        [HttpPost("Return-Replenish-Forms"), Authorize]
        public async Task<IActionResult> AddForm(NewFormDTO model)
        {
            var service = _context.Users.Include(u => u.ServiceRegistered).Where(u => u.UserName == User.Identity.Name).Select(u => u.ServiceRegistered).FirstOrDefault();
            var result = false;
            if (model.UpdateStock.Value)
            {
                if (!await _returnReplenishFormService.CheckStock(model))
                {
                    return BadRequest("Không đủ số lượng để xuất kho!");
                }
                result = await _returnReplenishFormService.NewFormExport(model, service);
            }
            else
            {
                result = await _returnReplenishFormService.AddForm(model, service);
            }
            if (result)
            {
                return Ok("OK");
            }
            else
                return BadRequest("Error");
        }
        [HttpDelete("Return-Replenish-Forms/{id}"), Authorize]
        public async Task<IActionResult> DeleteForm([FromRoute] string id)
        {
            if (await _returnReplenishFormService.DeleteForm(id))
                return Ok("OK");
            else
                return BadRequest("Error");
        }
        [HttpPut("Return-Replenish-Forms"), Authorize]
        public async Task<IActionResult> UpdateForm(NewFormDTO model)
        {
            if (await _returnReplenishFormService.UpdateForm(model))
                return Ok("OK");
            else
                return BadRequest("Error");
        }
        [HttpGet("Return-Replenish-Forms"), Authorize]
        public async Task<IActionResult> Forms()
        {
            var service = _context.Users.Include(u => u.ServiceRegistered)
                                        .Where(u => u.UserName == User.Identity.Name)
                                        .Select(u => u.ServiceRegistered)
                                        .FirstOrDefault();
            try
            {
                return Ok(await _returnReplenishFormService.GetAll(service));
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong!");
            }
        }
        [HttpGet("Return-Replenish-Forms/{id}"), Authorize]
        public async Task<IActionResult> Form(string id)
        {
            var service = _context.Users.Include(u => u.ServiceRegistered).Where(u => u.UserName == User.Identity.Name).Select(u => u.ServiceRegistered).FirstOrDefault();
            try
            {
                var result = await _returnReplenishFormService.FindById(service, id);
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
