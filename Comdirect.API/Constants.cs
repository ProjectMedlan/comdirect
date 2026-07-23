namespace Comdirect.API;
public class Constants
{
    public const string PRODUCT_TYPE_DEPOT = "DEPOT";
    public const string PRODUCT_TYPE_ACCOUNT = "ACCOUNT";
    public const string PRODUCT_TYPE_CARD = "CARD";

    public const string TAN_TYPE_PUSH_TAN = "P_TAN_PUSH";
    public const string TAN_TYPE_PHOTO_TAN = "P_TAN";
    public const string TAN_TYPE_MOBILE_TAN = "M_TAN";

    public const string BOOKING_STATUS_BOOKED = "BOOKED";
    public const string BOOKING_STATUS_NOT_BOOKED = "NOTBOOKED";

    public const string UNIT_XXX = "Stücke";
    public const string UNIT_XXC = "Prozent";
    public const string UNIT_XXM = "Promille";
    public const string UNIT_XXP = "Punkte";
    public const string UNIT_XXU = "Unbekannt";

    public const string TRANSACTION_DIRECTION_INCOMING = "IN";
    public const string TRANSACTION_DIRECTION_OUTGOING = "OUT";

    public static readonly Dictionary<string, string> ACCOUNT_TYPES = new()
    {
        { "FX", "Fremdwährungskonto" },
        { "OF", "Options-Futures-Konto" },
        { "CA", "Girokonto" },
        { "DAS", "Tagesgeld-Plus Konto" },
        { "CFD", "Contract for Difference Konto" },
        { "SA", "Tagesgeld/Verrechnungskonto" },
        { "LLA", "Wertpapier-Kreditkonto" },
    };

    public static readonly Dictionary<string, string> CARD_STATUS = new()
    {
        { "ACTIVE", "Aktiv" },
        { "INACTIVE", "Inaktiv" },
        { "IN_CHANGE", "Im Austausch" },
        { "UNKNOWN", "Unbekannt" }
    };

    public static readonly Dictionary<int, string> DOCUMENT_CATEGORIES = new()
    {
        { 301, "Nutzungsbedingungen (?)" },
        { 613, "Umtausch-/Barabfindungsangebot (?)" },
        { 320, "Werbung / Roboadvisor" },
        { 201, "Finanzreport" },
        { 252, "Steuermitteilung" },
        { 700, "Dividendengutschrift" }
    };

    public static readonly Dictionary<string, string> ACCOUNT_TRANSACTION_TYPES = new()
    {
        { "SECURITIES", "Wertpapiere" },
        { "CARD_TRANSACTION", "Kartenverfügung" },
        { "TRANSFER", "Überweisungen" },
        { "SAVINGS", "Termingelder" },
        { "INTEREST_DIVIDENDS", "Kupon" },
        { "ATM_WITHDRAWAL", "Bargeldauszahlung Geldautomat" },
        { "CANCELLATION", "Rücklastschriften" },
        { "DIRECT_DEBIT", "Lastschriften" },
    };

    public static readonly Dictionary<string, string> DEPOT_TRANSACTION_TYPES = new()
    {
        { "BUY", "Kauf" },
        { "SELL", "Verkauf" },
        { "TRANSFER_IN", "Einlieferung" },
        { "TRANSFER_OUT", "Auslieferung" }
    };
}
