using GiftOfTheGivers.Data;
using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using System.Text.Json;

namespace GiftOfTheGivers.Controllers
{
    public class DonationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public DonationsController(ApplicationDbContext context, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        // GET: Donations
        public async Task<IActionResult> Index()
        {
            return View(await _context.Donations.ToListAsync());
        }

        // GET: Donations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donation = await _context.Donations
                .FirstOrDefaultAsync(d => d.Id == id);

            if (donation == null)
            {
                return NotFound();
            }

            return View(donation);
        }

        // GET: Donations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Donations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,DonorName,Email,Amount,Currency,DonationType,IsAnonymous,DonationDate")]
                  Donation donation)
        {
            if (ModelState.IsValid)
            {
                // If the donor selected anonymous donation,
                // the application does not store their name.
                if (donation.IsAnonymous)
                {
                    donation.DonorName = "Anonymous";
                }

                // Records the date automatically.
                donation.DonationDate = DateTime.Now;

                // Save the donation first.
                _context.Add(donation);
                await _context.SaveChangesAsync();

                // Get the Azure Function URL from configuration.
                var functionUrl = _configuration[
                    "AzureFunctions:TaxCertificateUrl"];

                if (!string.IsNullOrWhiteSpace(functionUrl))
                {
                    try
                    {
                        var client = _httpClientFactory.CreateClient();

                        // Send donation information to the Azure Function.
                        var functionResponse = await client.PostAsJsonAsync(
                            functionUrl,
                            new
                            {
                                donorName = donation.DonorName,
                                amount = donation.Amount,
                                currency = donation.Currency,
                                donationType = donation.DonationType
                            });

                        if (functionResponse.IsSuccessStatusCode)
                        {
                            var certificate = await functionResponse
                                .Content
                                .ReadFromJsonAsync<TaxCertificateResponse>();

                            if (certificate != null)
                            {
                                TempData["TaxCertificateNumber"] =
                                    certificate.CertificateNumber;

                                TempData["TaxCertificateDate"] =
                                    certificate.Date;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // The donation has already been saved.
                        // If the Function is unavailable, the donation
                        // remains recorded in the system.

                        TempData["TaxCertificateError"] = $"Tax certificate generation failed: {ex.Message}";
                    }
                }

                // Sends the donor to the confirmation page.
                return RedirectToAction(
                    nameof(Confirmation),
                    new { id = donation.Id });
            }

            return View(donation);
        }

        // GET: Donations/Confirmation/5
        public async Task<IActionResult> Confirmation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donation = await _context.Donations
                .FirstOrDefaultAsync(d => d.Id == id);

            if (donation == null)
            {
                return NotFound();
            }

            return View(donation);
        }

        // GET: Donations/TaxCertificate/5
        public async Task<IActionResult> TaxCertificate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donation = await _context.Donations
                .FirstOrDefaultAsync(d => d.Id == id);

            if (donation == null)
            {
                return NotFound();
            }

            return View(donation);
        }

        // GET: Donations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donation = await _context.Donations.FindAsync(id);

            if (donation == null)
            {
                return NotFound();
            }

            return View(donation);
        }

        // POST: Donations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,DonorName,Email,Amount,Currency,DonationType,IsAnonymous,DonationDate")]
            Donation donation)
        {
            if (id != donation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (donation.IsAnonymous)
                {
                    donation.DonorName = "Anonymous";
                }

                try
                {
                    _context.Update(donation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonationExists(donation.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(donation);
        }

        // GET: Donations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donation = await _context.Donations
                .FirstOrDefaultAsync(d => d.Id == id);

            if (donation == null)
            {
                return NotFound();
            }

            return View(donation);
        }

        // POST: Donations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donation = await _context.Donations.FindAsync(id);

            if (donation != null)
            {
                _context.Donations.Remove(donation);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool DonationExists(int id)
        {
            return _context.Donations.Any(d => d.Id == id);
        }

        public class TaxCertificateResponse
        {

            public string CertificateNumber { get; set; } = "";

            public string Date { get; set; } = "";
        }
    }
}