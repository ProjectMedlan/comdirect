namespace Comdirect.ViewModelConverter;
internal static class ConverterHelper
{
    public static decimal ParseDecimal(string value)
    {
        if (string.IsNullOrEmpty(value)) return 0;

        // Repace dot with a comma (there's only one dot as decimal seperator due to the documentation)
        return decimal.Parse(value.Replace(".", ","));
    }

    public static DateOnly? ParseDate(string value)
    {
        if (string.IsNullOrEmpty(value)) return null;
        if (value.Length != 10) return null;

        return DateOnly.ParseExact(value, "yyyy-MM-dd", null);
    }
}
