
using GiftOfTheGivers.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GiftOfTheGivers.Tests
{
    [TestClass]
    public class DonationHelperTests
    {
        [TestMethod]
        public void CalculateDonationTotal_AddsAmountsCorrectly()
        {
            decimal result =
                DonationHelper.CalculateDonationTotal(500m, 150m);

            Assert.AreEqual(650m, result);
        }

        [TestMethod]
        public void FormatTaxCertificateNumber_TrimsAndUppercases()
        {
            string result =
                DonationHelper.FormatTaxCertificateNumber("  gotg-ab123  ");

            Assert.AreEqual("GOTG-AB123", result);
        }

        [TestMethod]
        public void FormatTaxCertificateNumber_EmptyValueReturnsNA()
        {
            string result =
                DonationHelper.FormatTaxCertificateNumber("   ");

            Assert.AreEqual("N/A", result);
        }
    }
}
