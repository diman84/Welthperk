namespace Wealthperk.Web.Formatting
{
    public static class FormattingHelper
    {
        public static string FormatCurrency(this double? value, string cur = "$")
        {
           switch(value){
                 case null:
                 case var val when val == 0:
                 default:
                    return "N/A";
                case var val when val > 0:
                    return FormatCurrency(val.Value, cur);
                case var val when val < 0:
                    return FormatCurrency(val.Value, cur);
             }
        }

        public static string FormatPercentageWithSign(this double? value, string cur = "$")
        {
            switch(value) {
                 case null:
                 case var val when val == 0:
                 default:
                    return "N/A";
                case var val when val > 0:
                    return "+ " + FormatPercentage(val.Value, cur);
                case var val when val < 0:
                    return "- " + FormatPercentage(-val.Value, cur);
             }
        }

        public static string FormatCurrencyWithNoSign(this double? value, string cur = "$")
        {
            switch(value){
                 case null:
                 case var val when val == 0:
                 default:
                    return "N/A";
                case var val when val > 0:
                    return FormatCurrency(val.Value, cur);
                case var val when val < 0:
                    return FormatCurrency(-val.Value, cur);
             }
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

        public static string FormatCurrencyWithSign(this double? value, string cur = "$")
        {
            switch(value){
                 case null:
                 case var val when val == 0:
                 default:
                    return "N/A";
                case var val when val > 0:
                    return "+ " + FormatCurrency(val.Value, cur);
                case var val when val < 0:
                    return "- " + FormatCurrency(-val.Value, cur);
             }
        }


        private static string FormatCurrency(this double value, string cur = "$")
        {
             return string.Format("{0}{1:#,#.##}", cur, value);
        }
    }
}