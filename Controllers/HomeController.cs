using AcxiomCRM.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace AcxiomCRM.Controllers;

[Authorize(Roles = "Admin,Manager,SalesExecutive")]
public class HomeController(ApplicationDbContext db) : Controller
{
    public async Task<IActionResult> Index()
    {
        ViewBag.Customers = await db.Customers.CountAsync();
        ViewBag.Leads = await db.Leads.CountAsync();
        ViewBag.OpenOpps = await db.Opportunities.CountAsync(x => x.Status == "Open");
        ViewBag.Won = await db.Opportunities.CountAsync(x => x.Stage == "Won");

        var openAmounts = await db.Opportunities
            .Where(x => x.Status == "Open")
            .Select(x => x.Amount)
            .ToListAsync();

        ViewBag.Pipeline = openAmounts.Sum();

        ViewBag.LeadLabels = new[] { "New", "Contacted", "Qualified", "Converted", "Lost" };

        ViewBag.LeadValues = await Task.WhenAll(
            new[] { "New", "Contacted", "Qualified", "Converted", "Lost" }
                .Select(s => db.Leads.CountAsync(x => x.Status == s))
        );

        return View();
    }
}
