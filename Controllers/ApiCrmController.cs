using AcxiomCRM.Data;
using AcxiomCRM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcxiomCRM.Controllers;

[ApiController]
[Route("api")]
[Authorize(Roles = "Admin,Manager,SalesExecutive")]
public class ApiCrmController(ApplicationDbContext db) : ControllerBase
{
    [HttpGet("customers")]
    public async Task<IActionResult> Customers() =>
        Ok(await db.Customers.Select(x => new { x.Id, x.CustomerName, x.Email, x.Phone, x.CompanyName, x.Status }).ToListAsync());

    [HttpGet("customers/{id:int}")]
    public async Task<IActionResult> Customer(int id)
    {
        var x = await db.Customers.FindAsync(id);
        return x == null ? NotFound() : Ok(new { x.Id, x.CustomerName, x.Email, x.Phone, x.CompanyName, x.Status });
    }

    [HttpPost("customers")]
    public async Task<IActionResult> CreateCustomer([FromBody] Customer input)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        if (await db.Customers.AnyAsync(x => x.Email == input.Email || x.Phone == input.Phone))
            return Conflict(new { message = "A customer with this email or phone already exists." });
        var customer = new Customer { CustomerName = input.CustomerName, Email = input.Email, Phone = input.Phone,
            CompanyName = input.CompanyName, Address = input.Address, City = input.City, State = input.State };
        db.Customers.Add(customer);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(Customer), new { id = customer.Id },
            new { customer.Id, customer.CustomerName, customer.Email, customer.Phone });
    }

    [HttpPut("customers/{id:int}")]
    public async Task<IActionResult> UpdateCustomer(int id, [FromBody] Customer input)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var x = await db.Customers.FindAsync(id);
        if (x == null) return NotFound();
        if (await db.Customers.AnyAsync(c => c.Id != id && (c.Email == input.Email || c.Phone == input.Phone)))
            return Conflict(new { message = "Email or phone already belongs to another customer." });
        x.CustomerName = input.CustomerName; x.Email = input.Email; x.Phone = input.Phone;
        x.CompanyName = input.CompanyName; x.Address = input.Address; x.City = input.City; x.State = input.State;
        await db.SaveChangesAsync();
        return Ok(new { x.Id, x.CustomerName, x.Email, x.Phone });
    }

    [HttpDelete("customers/{id:int}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        var x = await db.Customers.FindAsync(id);
        if (x == null) return NotFound();
        x.Status = "Inactive";
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("leads")]
    public async Task<IActionResult> Leads() =>
        Ok(await db.Leads.Select(x => new { x.Id, x.LeadName, x.Email, x.Phone, x.Source, x.Status, x.ExpectedValue }).ToListAsync());

    [HttpPost("leads")]
    public async Task<IActionResult> CreateLead([FromBody] Lead input)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        db.Leads.Add(input);
        await db.SaveChangesAsync();
        return Created($"/api/leads/{input.Id}", new { input.Id, input.LeadName, input.Status });
    }

    [HttpGet("opportunities")]
    public async Task<IActionResult> Opportunities() =>
        Ok(await db.Opportunities.Select(x => new { x.Id, x.OpportunityName, x.Amount, x.Probability, x.Stage, x.ExpectedCloseDate }).ToListAsync());

    [HttpPost("opportunities")]
    public async Task<IActionResult> CreateOpportunity([FromBody] Opportunity input)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        if (input.Amount <= 0) return BadRequest(new { message = "Amount must be greater than zero." });
        if (input.Probability is < 0 or > 100) return BadRequest(new { message = "Probability must be between 0 and 100." });
        if (input.Stage != "Won" && input.Stage != "Lost" && input.ExpectedCloseDate.Date < DateTime.Today)
            return BadRequest(new { message = "Expected close date cannot be in the past." });
        db.Opportunities.Add(input);
        await db.SaveChangesAsync();
        return Created($"/api/opportunities/{input.Id}", new { input.Id, input.OpportunityName });
    }
}
