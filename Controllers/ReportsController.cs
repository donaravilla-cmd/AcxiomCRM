using AcxiomCRM.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcxiomCRM.Controllers;

[Authorize(Roles = "Admin,Manager")]
public class ReportsController(ApplicationDbContext db) : Controller
{
    public async Task<IActionResult> Index()
    {
        ViewBag.CustomerCount = await db.Customers.CountAsync();
        ViewBag.LeadCount = await db.Leads.CountAsync();
        ViewBag.OpportunityCount = await db.Opportunities.CountAsync();

        var openPipelineAmounts = await db.Opportunities
            .Where(x => x.Status == "Open" && x.Stage != "Won" && x.Stage != "Lost")
            .Select(x => x.Amount)
            .ToListAsync();

        ViewBag.OpenPipeline = openPipelineAmounts.Sum();

        var wonAmounts = await db.Opportunities
            .Where(x => x.Stage == "Won")
            .Select(x => x.Amount)
            .ToListAsync();

        ViewBag.WonValue = wonAmounts.Sum();

        ViewBag.OverdueFollowUps = await db.FollowUps
            .CountAsync(x => x.Status == "Planned" && x.FollowUpDate < DateTime.Today);

        ViewBag.Customers = await db.Customers
            .OrderByDescending(x => x.CreatedDate)
            .Take(10)
            .ToListAsync();

        return View();
    }
}
