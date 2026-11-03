using AcxiomCRM.Data; using AcxiomCRM.Models; using AcxiomCRM.Services; using Microsoft.AspNetCore.Authorization; using Microsoft.AspNetCore.Mvc; using Microsoft.EntityFrameworkCore;
namespace AcxiomCRM.Controllers;
[Authorize(Roles = "Admin,Manager,SalesExecutive")] public class CustomersController(ApplicationDbContext db,AuditService audit):Controller {
 public async Task<IActionResult> Index(string? q){var data=db.Customers.AsQueryable();if(!string.IsNullOrWhiteSpace(q))data=data.Where(x=>x.CustomerName.Contains(q)||x.Email.Contains(q)||x.Phone.Contains(q)|| (x.CompanyName??"").Contains(q));return View(await data.OrderByDescending(x=>x.CreatedDate).ToListAsync());}
 public IActionResult Create()=>View(new Customer());
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Create(Customer model){if(await db.Customers.AnyAsync(x=>x.Email==model.Email||x.Phone==model.Phone))ModelState.AddModelError("","Email or phone already exists.");if(!ModelState.IsValid)return View(model);db.Customers.Add(model);await db.SaveChangesAsync();await audit.LogAsync("Create","Customer",model.Id.ToString(),model.CustomerName);return RedirectToAction(nameof(Index));}
 public async Task<IActionResult> Edit(int id){var x=await db.Customers.FindAsync(id);return x==null?NotFound():View(x);}
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Edit(int id,Customer model){if(id!=model.Id)return BadRequest();if(!ModelState.IsValid)return View(model);db.Update(model);await db.SaveChangesAsync();await audit.LogAsync("Update","Customer",id.ToString(),model.CustomerName);return RedirectToAction(nameof(Index));}
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Delete(int id){var x=await db.Customers.FindAsync(id);if(x!=null){db.Customers.Remove(x);await db.SaveChangesAsync();await audit.LogAsync("Delete","Customer",id.ToString(),x.CustomerName);}return RedirectToAction(nameof(Index));}
}
