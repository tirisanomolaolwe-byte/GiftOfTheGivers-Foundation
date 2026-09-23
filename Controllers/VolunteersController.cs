
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using GiftOfTheGivers.Models;
using GiftOfTheGivers.Data;

public class VolunteersController : Controller
{
    private readonly ApplicationDbContext _context;

    public VolunteersController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: VOLUNTEERS
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Volunteers.ToListAsync());
    }

    // GET: VOLUNTEERS/Details/5
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var volunteer = await _context.Volunteers
            .FirstOrDefaultAsync(m => m.Id == id);
        if (volunteer == null)
        {
            return NotFound();
        }

        return View(volunteer);
    }

    // GET: VOLUNTEERS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: VOLUNTEERS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,FullName,Email,Skills,Availability,DateRegistered")] Volunteer volunteer)
    {
        if (ModelState.IsValid)
        {
            _context.Add(volunteer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(volunteer);
    }

    // GET: VOLUNTEERS/Edit/5
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var volunteer = await _context.Volunteers.FindAsync(id);
        if (volunteer == null)
        {
            return NotFound();
        }
        return View(volunteer);
    }

    // POST: VOLUNTEERS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,FullName,Email,Skills,Availability,DateRegistered")] Volunteer volunteer)
    {
        if (id != volunteer.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(volunteer);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VolunteerExists(volunteer.Id))
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
        return View(volunteer);
    }

    // GET: VOLUNTEERS/Delete/5
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var volunteer = await _context.Volunteers
            .FirstOrDefaultAsync(m => m.Id == id);
        if (volunteer == null)
        {
            return NotFound();
        }

        return View(volunteer);
    }

    // POST: VOLUNTEERS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var volunteer = await _context.Volunteers.FindAsync(id);
        if (volunteer != null)
        {
            _context.Volunteers.Remove(volunteer);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool VolunteerExists(int? id)
    {
        return _context.Volunteers.Any(e => e.Id == id);
    }
}
