
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using GiftOfTheGivers.Models;
using GiftOfTheGivers.Data;

public class ReliefProjectsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ReliefProjectsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: RELIEFPROJECTS
    public async Task<IActionResult> Index()    
    {
        var model = await _context.ReliefProjects.ToListAsync();
        return View("~/Views/ReliefProjects/Index.cshtml", model);
    }

    // GET: RELIEFPROJECTS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var reliefproject = await _context.ReliefProjects
            .FirstOrDefaultAsync(m => m.Id == id);
        if (reliefproject == null)
        {
            return NotFound();
        }

        return View("~/Views/ReliefProjects/Details.cshtml", reliefproject);
    }

    // GET: RELIEFPROJECTS/Create
    [Authorize(Roles = "Employee")]
    public IActionResult Create()
    {
        return View("~/Views/ReliefProjects/Create.cshtml");
    }

    // POST: RELIEFPROJECTS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> Create([Bind("Id,Name,Location,Description,Status,DateStarted,LastUpdated")] ReliefProject reliefproject)
    {
        if (ModelState.IsValid)
        {
            _context.Add(reliefproject);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View("~/Views/ReliefProjects/Create.cshtml", reliefproject);
    }

    // GET: RELIEFPROJECTS/Edit/5
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var reliefproject = await _context.ReliefProjects.FindAsync(id);
        if (reliefproject == null)
        {
            return NotFound();
        }
        return View("~/Views/ReliefProjects/Edit.cshtml", reliefproject);
    }

    // POST: RELIEFPROJECTS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Name,Location,Description,Status,DateStarted,LastUpdated")] ReliefProject reliefproject)
    {
        if (id != reliefproject.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(reliefproject);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReliefProjectExists(reliefproject.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View("~/Views/ReliefProjects/Edit.cshtml", reliefproject);
    }

    // GET: RELIEFPROJECTS/Delete/5
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var reliefproject = await _context.ReliefProjects
            .FirstOrDefaultAsync(m => m.Id == id);
        if (reliefproject == null)
        {
            return NotFound();
        }

        return View("~/Views/ReliefProjects/Delete.cshtml", reliefproject);
    }

    // POST: RELIEFPROJECTS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var reliefproject = await _context.ReliefProjects.FindAsync(id);
        if (reliefproject != null)
        {
            _context.ReliefProjects.Remove(reliefproject);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ReliefProjectExists(int? id)
    {
        return _context.ReliefProjects.Any(e => e.Id == id);
    }
}
