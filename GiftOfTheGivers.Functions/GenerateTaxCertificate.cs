using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace GiftOfTheGivers.Functions
{
    public class GenerateTaxCertificate
    {
        [Function("GenerateTaxCertificate")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")]
            HttpRequest req)
        {
            try
            {
                var donation = await JsonSerializer.DeserializeAsync<DonationRequest>(
                    req.Body,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (donation == null)
                {
                    return new BadRequestObjectResult(
                        "Donation information is required.");
                }

                var certificate = new
                {
                    message = "Dummy tax certificate generated successfully.",
                    certificateNumber = "GOTG-" +
                                       DateTime.Now.ToString("yyyyMMddHHmmss"),
                    organisation = "Gift of the Givers Foundation",
                    donor = donation.DonorName,
                    donationAmount = donation.Amount,
                    currency = donation.Currency,
                    donationType = donation.DonationType,
                    date = DateTime.Now.ToString("yyyy-MM-dd")
                };

                return new OkObjectResult(certificate);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(
                    $"Unable to generate certificate: {ex.Message}");
            }
        }
    }

    public class DonationRequest
    {
        public string DonorName { get; set; } = "";
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "ZAR";
        public string DonationType { get; set; } = "One-Time";
    }
}