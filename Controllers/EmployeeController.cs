using GiftOfTheGivers.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace GiftOfTheGivers.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Employee
        public async Task<IActionResult> Index()
        {
            var projectCount = await _context.ReliefProjects.CountAsync();
            var volunteerCount = await _context.Volunteers.CountAsync();
            var donationCount = await _context.Donations.CountAsync();

            ViewBag.ProjectCount = projectCount;
            ViewBag.VolunteerCount = volunteerCount;
            ViewBag.DonationCount = donationCount;

            return View();
        }

        // GET: Employee/Volunteers
        public async Task<IActionResult> Volunteers()
        {
            var volunteers = await _context.Volunteers
                .OrderByDescending(v => v.DateRegistered)
                .ToListAsync();

            return View(volunteers);
        }

        // GET: Employee/ReliefProjects
        public async Task<IActionResult> ReliefProjects()
        {
            var projects = await _context.ReliefProjects
                .OrderByDescending(p => p.LastUpdated)
                .ToListAsync();

            return View(projects);
        }
    }
}