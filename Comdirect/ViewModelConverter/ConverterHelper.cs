using System.Globalization;

namespace Comdirect.ViewModelConverter;
internal static class ConverterHelper
{
    public static decimal ParseDecimal(string value)
    {
        if (string.IsNullOrEmpty(value)) return 0;

        // The API always uses a dot as decimal separator (see documentation)
        return decimal.Parse(value, CultureInfo.InvariantCulture);
    }

    public static DateOnly? ParseDate(string value)
    {
        if (string.IsNullOrEmpty(value)) return null;
        if (value.Length != 10) return null;

        return DateOnly.ParseExact(value, "yyyy-MM-dd", null);
    }
}
