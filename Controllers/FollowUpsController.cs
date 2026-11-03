using AcxiomCRM.Data;using AcxiomCRM.Models;using AcxiomCRM.Services;using Microsoft.AspNetCore.Authorization;using Microsoft.AspNetCore.Mvc;using Microsoft.EntityFrameworkCore;
namespace AcxiomCRM.Controllers;
[Authorize(Roles = "Admin,Manager,SalesExecutive")] public class FollowUpsController(ApplicationDbContext db,AuditService audit):Controller{
 public async Task<IActionResult> Index()=>View(await db.FollowUps.OrderBy(x=>x.FollowUpDate).ToListAsync()); public IActionResult Create()=>View(new FollowUp());
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Create(FollowUp x){if(x.FollowUpDate.Date<DateTime.Today)ModelState.AddModelError(nameof(x.FollowUpDate),"Follow-up date cannot be earlier than today.");if(!ModelState.IsValid)return View(x);db.FollowUps.Add(x);await db.SaveChangesAsync();await audit.LogAsync("Create","FollowUp",x.Id.ToString(),x.FollowUpType);return RedirectToAction(nameof(Index));}
}
