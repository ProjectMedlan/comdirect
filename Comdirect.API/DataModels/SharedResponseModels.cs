namespace Comdirect.API.DataModels;
public class ComdirectInstrument
{
    public string instrumentId { get; set; } = null!;
    public string wkn { get; set; } = null!;
    public string isin { get; set; } = null!;
    public string mnemonic { get; set; } = null!;
    public string name { get; set; } = null!;
    public string shortName { get; set; } = null!;
    public ComdirectInstrumentStaticdata staticData { get; set; } = null!;
}

public class ComdirectInstrumentStaticdata
{
    public string notation { get; set; } = null!;
    public string currency { get; set; } = null!;
    public string instrumentType { get; set; } = null!;
    public bool priipsRelevant { get; set; }
    public bool kidAvailable { get; set; }
    public bool shippingWaiverRequired { get; set; }
    public bool fundRedemptionLimited { get; set; }
    public string savingsPlanEligibility { get; set; } = null!;

    /// <summary>Feld Sector</summary>
    /// <remarks> Bei den Deportransaktionen evtl. nicht mit dabei</remarks>
    public string? sector { get; set; }
}

/// <summary>Basisklasse für die Paging-Informationen</summary>
public class ComdirectPaging
{
    public int? index { get; set; }
    public int matches { get; set; }
}
public class Balance
{
    public string value { get; set; } = null!;
    public string unit { get; set; } = null!;
}