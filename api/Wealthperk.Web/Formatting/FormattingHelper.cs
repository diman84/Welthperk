namespace Wealthperk.Web.Formatting
{
    public static class FormattingHelper
    {
        public static string FormatCurrency(this double? value, string cur = "$")
        {
            if (value == null)
                return "N/A";

           return FormatCurrency(value.Value, cur);
        }

        public static string FormatCurrency(this double value, string cur = "$")
        {
             return string.Format("{0}{1:n}", cur, value);
        }

        public static string FormatCurrency(this int? value, string cur = "$")
        {
              if (value == null)
                return "N/A";

             return string.Format("{0}{1:n}", cur, value.Value);
        }

        public static string FormatPercentage(this double? value, string cur = "$")
        {
              if (value == null)
                return "N/A";

             return string.Format("{0:P2}", value.Value);
        }
    }
}