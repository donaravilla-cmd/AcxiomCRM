using AcxiomCRM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AcxiomCRM.Controllers;

[Authorize(Roles = "Admin")]
public class UsersController(
    UserManager<ApplicationUser> userManager) : Controller
{
    public async Task<IActionResult> Index()
    {
        var users = userManager.Users.OrderBy(x => x.Email).ToList();
        var result = new List<UserViewModel>();

        foreach (var user in users)
        {
            var roles = await userManager.GetRolesAsync(user);

            result.Add(new UserViewModel
            {
                Id = user.Id,
                FullName = user.FullName ?? "",
                Email = user.Email ?? "",
                Role = roles.FirstOrDefault() ?? "No Role"
            });
        }

        return View(result);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeRole(string id, string role)
    {
        if (role != "Manager" && role != "SalesExecutive")
            return BadRequest("Invalid role.");

        var user = await userManager.FindByIdAsync(id);

        if (user == null)
            return NotFound();

        var currentRoles = await userManager.GetRolesAsync(user);

        if (currentRoles.Contains("Admin"))
            return BadRequest("Admin roles cannot be changed here.");

        var result = await userManager.RemoveFromRolesAsync(user, currentRoles);

        if (!result.Succeeded)
            return BadRequest("Could not remove existing role.");

        result = await userManager.AddToRoleAsync(user, role);

        if (!result.Succeeded)
            return BadRequest("Could not assign new role.");

        TempData["Message"] = "User role updated successfully.";
        return RedirectToAction(nameof(Index));
    }
}

public class UserViewModel
{
    public string Id { get; set; } = "";
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Role { get; set; } = "";
}
