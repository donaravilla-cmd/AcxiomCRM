using AcxiomCRM.Data;using AcxiomCRM.Models;using AcxiomCRM.Services;using Microsoft.AspNetCore.Authorization;using Microsoft.AspNetCore.Mvc;using Microsoft.EntityFrameworkCore;
namespace AcxiomCRM.Controllers;
[Authorize(Roles = "Admin,Manager,SalesExecutive")] public class OpportunitiesController(ApplicationDbContext db,AuditService audit):Controller{
 public async Task<IActionResult> Index()=>View(await db.Opportunities.OrderByDescending(x=>x.Id).ToListAsync()); public IActionResult Create()=>View(new Opportunity());
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Create(Opportunity x){if(x.Amount<=0)ModelState.AddModelError(nameof(x.Amount),"Amount must be greater than zero.");if(x.ExpectedCloseDate.Date<DateTime.Today&&x.Status=="Open")ModelState.AddModelError(nameof(x.ExpectedCloseDate),"Close date cannot be in the past.");if(!ModelState.IsValid)return View(x);db.Opportunities.Add(x);await db.SaveChangesAsync();await audit.LogAsync("Create","Opportunity",x.Id.ToString(),x.OpportunityName);return RedirectToAction(nameof(Index));}
}
