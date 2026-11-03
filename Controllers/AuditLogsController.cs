using AcxiomCRM.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcxiomCRM.Controllers;

[Authorize(Roles = "Admin")]
public class AuditLogsController(ApplicationDbContext db) : Controller
{
    public async Task<IActionResult> Index(string? module, string? actionName)
    {
        var query = db.AuditLogs.AsQueryable();
        if (!string.IsNullOrWhiteSpace(module)) query = query.Where(x => x.EntityName.Contains(module));
        if (!string.IsNullOrWhiteSpace(actionName)) query = query.Where(x => x.Action.Contains(actionName));
        return View(await query.OrderByDescending(x => x.CreatedDate).Take(500).ToListAsync());
    }
}
