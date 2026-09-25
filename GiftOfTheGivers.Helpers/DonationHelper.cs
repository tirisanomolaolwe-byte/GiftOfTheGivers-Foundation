namespace GiftOfTheGivers.Helpers
{
    public static class DonationHelper
    {
        public static string FormatTaxCertificateNumber(string certificateNumber)
        {
            if (string.IsNullOrWhiteSpace(certificateNumber))
            {
                return "N/A";
            }

            return certificateNumber.Trim().ToUpper();
        }

        public static decimal CalculateDonationTotal(
            decimal amount,
            decimal additionalAmount = 0)
        {
            return amount + additionalAmount;
        }
    }
}