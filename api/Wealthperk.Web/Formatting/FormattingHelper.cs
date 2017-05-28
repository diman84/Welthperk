namespace Wealthperk.Web.Formatting
{
    public static class FormattingHelper
    {
        public static string FormatCurrency(this double? value, string cur = "$")
        {
            if (value == null)
                return "N/A";

            return string.Format("{0}{1:n}", cur, value.Value);
        }
    }
}