using AcxiomCRM.Data;
using AcxiomCRM.Models;
using System.Security.Claims;

namespace AcxiomCRM.Services;

public class AuditService(ApplicationDbContext db, IHttpContextAccessor accessor)
{
    public async Task LogAsync(string action, string entity, string? recordId = null, string? details = null)
    {
        var context = accessor.HttpContext;
        db.AuditLogs.Add(new AuditLog
        {
            Action = action,
            EntityName = entity,
            RecordId = recordId,
            NewValue = details,
            UserId = context?.User?.FindFirstValue(ClaimTypes.NameIdentifier),
            IpAddress = context?.Connection.RemoteIpAddress?.ToString(),
            CreatedDate = DateTime.UtcNow
        });
        await db.SaveChangesAsync();
    }
}
