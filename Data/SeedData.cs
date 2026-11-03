using AcxiomCRM.Models;

namespace AcxiomCRM.Data;

public static class SeedData
{
    public static async Task InitializeAsync(ApplicationDbContext db)
    {
        // Customers: preserve existing records and avoid duplicate emails.
        var customers = new[]
        {
            new Customer { CustomerName = "Priya Sharma", Email = "priya@techvision.com", Phone = "9876543211", CompanyName = "TechVision", City = "Bengaluru", State = "Karnataka" },
            new Customer { CustomerName = "Arjun Reddy", Email = "arjun@innovate.com", Phone = "9876543212", CompanyName = "Innovate Solutions", City = "Hyderabad", State = "Telangana" },
            new Customer { CustomerName = "Sneha Patel", Email = "sneha@bright.com", Phone = "9876543213", CompanyName = "Bright Enterprises", City = "Mumbai", State = "Maharashtra" },
            new Customer { CustomerName = "Kiran Kumar", Email = "kiran@nextgen.com", Phone = "9876543214", CompanyName = "NextGen Systems", City = "Visakhapatnam", State = "Andhra Pradesh" }
        };

        foreach (var customer in customers)
            if (!db.Customers.Any(x => x.Email == customer.Email))
                db.Customers.Add(customer);

        await db.SaveChangesAsync();

        var customerIds = db.Customers.OrderBy(x => x.Id).Select(x => x.Id).ToList();
        var firstCustomer = customerIds.FirstOrDefault();

        // Leads
        if (!db.Leads.Any())
        {
            db.Leads.AddRange(
                new Lead { LeadName = "Rohit Verma", Email = "rohit@example.com", Phone = "9876500001", CompanyName = "Alpha Corp", Source = "Website", Status = "New", ExpectedValue = 150000 },
                new Lead { LeadName = "Ananya Rao", Email = "ananya@example.com", Phone = "9876500002", CompanyName = "Beta Ltd", Source = "Referral", Status = "Contacted", ExpectedValue = 250000 },
                new Lead { LeadName = "Vikram Singh", Email = "vikram@example.com", Phone = "9876500003", CompanyName = "CloudWorks", Source = "LinkedIn", Status = "Qualified", ExpectedValue = 400000 },
                new Lead { LeadName = "Meera Nair", Email = "meera@example.com", Phone = "9876500004", CompanyName = "DataPro", Source = "Campaign", Status = "New", ExpectedValue = 180000 },
                new Lead { LeadName = "Sanjay Das", Email = "sanjay@example.com", Phone = "9876500005", CompanyName = "FinEdge", Source = "Website", Status = "Converted", ExpectedValue = 300000 },
                new Lead { LeadName = "Divya Menon", Email = "divya@example.com", Phone = "9876500006", CompanyName = "SmartTech", Source = "Referral", Status = "Lost", ExpectedValue = 100000 }
            );
            await db.SaveChangesAsync();
        }

        var leadIds = db.Leads.OrderBy(x => x.Id).Select(x => x.Id).ToList();
        var firstLead = leadIds.FirstOrDefault();

        // Opportunities
        if (!db.Opportunities.Any())
        {
            db.Opportunities.AddRange(
                new Opportunity { OpportunityName = "CRM Implementation", CustomerId = firstCustomer, Amount = 500000, Probability = 80, Stage = "Proposal", ExpectedCloseDate = DateTime.Today.AddDays(20) },
                new Opportunity { OpportunityName = "Cloud Migration", CustomerId = customerIds.Skip(1).FirstOrDefault(), Amount = 750000, Probability = 60, Stage = "Negotiation", ExpectedCloseDate = DateTime.Today.AddDays(30) },
                new Opportunity { OpportunityName = "Analytics Platform", CustomerId = customerIds.Skip(2).FirstOrDefault(), Amount = 350000, Probability = 90, Stage = "Won", Status = "Closed", ExpectedCloseDate = DateTime.Today.AddDays(-5) },
                new Opportunity { OpportunityName = "ERP Upgrade", CustomerId = customerIds.Skip(3).FirstOrDefault(), Amount = 250000, Probability = 30, Stage = "Qualification", ExpectedCloseDate = DateTime.Today.AddDays(45) },
                new Opportunity { OpportunityName = "Mobile Application", CustomerId = firstCustomer, Amount = 180000, Probability = 20, Stage = "Lost", Status = "Closed", ExpectedCloseDate = DateTime.Today.AddDays(-10) }
            );
            await db.SaveChangesAsync();
        }

        // Follow-ups
        if (!db.FollowUps.Any())
        {
            db.FollowUps.AddRange(
                new FollowUp { CustomerId = firstCustomer, FollowUpDate = DateTime.Today.AddDays(1), FollowUpType = "Call", Remarks = "Discuss project requirements" },
                new FollowUp { CustomerId = customerIds.Skip(1).FirstOrDefault(), FollowUpDate = DateTime.Today.AddDays(2), FollowUpType = "Meeting", Remarks = "Product demonstration" },
                new FollowUp { LeadId = firstLead, FollowUpDate = DateTime.Today.AddDays(3), FollowUpType = "Email", Remarks = "Send pricing proposal" },
                new FollowUp { CustomerId = customerIds.Skip(2).FirstOrDefault(), FollowUpDate = DateTime.Today.AddDays(5), FollowUpType = "Call", Remarks = "Check customer feedback" },
                new FollowUp { LeadId = leadIds.Skip(1).FirstOrDefault(), FollowUpDate = DateTime.Today.AddDays(7), FollowUpType = "Meeting", Remarks = "Discuss next steps" }
            );
            await db.SaveChangesAsync();
        }

        // Activities
        if (!db.Activities.Any())
        {
            db.Activities.AddRange(
                new Activity { ActivityType = "Call", Subject = "Initial customer discussion", Description = "Discussed business requirements", CustomerId = firstCustomer },
                new Activity { ActivityType = "Meeting", Subject = "Product demonstration", Description = "Presented CRM features", CustomerId = customerIds.Skip(1).FirstOrDefault() },
                new Activity { ActivityType = "Email", Subject = "Proposal shared", Description = "Sent commercial proposal", LeadId = firstLead },
                new Activity { ActivityType = "Call", Subject = "Lead qualification", Description = "Discussed project budget", LeadId = leadIds.Skip(1).FirstOrDefault() },
                new Activity { ActivityType = "Meeting", Subject = "Project review", Description = "Reviewed implementation plan", CustomerId = customerIds.Skip(2).FirstOrDefault() },
                new Activity { ActivityType = "Email", Subject = "Follow-up email", Description = "Shared project updates", CustomerId = customerIds.Skip(3).FirstOrDefault() }
            );
            await db.SaveChangesAsync();
        }

        // Demo audit entries
        if (!db.AuditLogs.Any())
        {
            db.AuditLogs.AddRange(
                new AuditLog { Action = "Create", EntityName = "Customer", RecordId = "DEMO-1", NewValue = "Priya Sharma", IpAddress = "127.0.0.1" },
                new AuditLog { Action = "Create", EntityName = "Lead", RecordId = "DEMO-2", NewValue = "Rohit Verma", IpAddress = "127.0.0.1" },
                new AuditLog { Action = "Create", EntityName = "Opportunity", RecordId = "DEMO-3", NewValue = "CRM Implementation", IpAddress = "127.0.0.1" }
            );
            await db.SaveChangesAsync();
        }
    }
}
