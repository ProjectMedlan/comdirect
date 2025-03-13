namespace Comdirect.API.DataModels;
public class ComdirectInstrument
{
    public string instrumentId { get; set; }
    public string wkn { get; set; }
    public string isin { get; set; }
    public string mnemonic { get; set; }
    public string name { get; set; }
    public string shortName { get; set; }
    public ComdirectInstrumentStaticdata staticData { get; set; }
}

public class ComdirectInstrumentStaticdata
{
    public string notation { get; set; }
    public string currency { get; set; }
    public string instrumentType { get; set; }
    public bool priipsRelevant { get; set; }
    public bool kidAvailable { get; set; }
    public bool shippingWaiverRequired { get; set; }
    public bool fundRedemptionLimited { get; set; }
    public string savingsPlanEligibility { get; set; }

    /// <summary>Feld Sector</summary>
    /// <remarks> Bei den Deportransaktionen evtl. nicht mit dabei</remarks>
    public string? sector { get; set; }
}

/// <summary>Basisklasse für die Paging-Informationen</summary>
public class ComdirectPaging
{
    public int index { get; set; }
    public int matches { get; set; }
}
public class Balance
{
    public string value { get; set; }
    public string unit { get; set; }
}