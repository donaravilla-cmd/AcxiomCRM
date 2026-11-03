using AcxiomCRM.Data;using AcxiomCRM.Models;using AcxiomCRM.Services;using Microsoft.AspNetCore.Authorization;using Microsoft.AspNetCore.Mvc;using Microsoft.EntityFrameworkCore;
namespace AcxiomCRM.Controllers;
[Authorize(Roles = "Admin,Manager,SalesExecutive")] public class LeadsController(ApplicationDbContext db,AuditService audit):Controller{
 public async Task<IActionResult> Index()=>View(await db.Leads.OrderByDescending(x=>x.CreatedDate).ToListAsync()); public IActionResult Create()=>View(new Lead());
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Create(Lead x){if(!ModelState.IsValid)return View(x);db.Leads.Add(x);await db.SaveChangesAsync();await audit.LogAsync("Create","Lead",x.Id.ToString(),x.LeadName);return RedirectToAction(nameof(Index));}
}
