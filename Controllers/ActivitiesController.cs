using AcxiomCRM.Data;
using AcxiomCRM.Models;
using AcxiomCRM.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcxiomCRM.Controllers;

[Authorize(Roles = "Admin,Manager,SalesExecutive")]
public class ActivitiesController(ApplicationDbContext db, AuditService audit) : Controller
{
    public async Task<IActionResult> Index() =>
        View(await db.Activities.OrderByDescending(x => x.ActivityDate).ToListAsync());

    public IActionResult Create() => View(new Activity());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Activity model)
    {
        if (!ModelState.IsValid) return View(model);
        db.Activities.Add(model);
        await db.SaveChangesAsync();
        await audit.LogAsync("Create", "Activity", model.Id.ToString(), model.Subject);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var item = await db.Activities.FindAsync(id);
        return item == null ? NotFound() : View(item);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Activity model)
    {
        if (id != model.Id) return BadRequest();
        if (!ModelState.IsValid) return View(model);
        db.Activities.Update(model);
        await db.SaveChangesAsync();
        await audit.LogAsync("Update", "Activity", id.ToString(), model.Subject);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await db.Activities.FindAsync(id);
        if (item != null) {
            db.Activities.Remove(item);
            await db.SaveChangesAsync();
            await audit.LogAsync("Delete", "Activity", id.ToString(), item.Subject);
        }
        return RedirectToAction(nameof(Index));
    }
}
