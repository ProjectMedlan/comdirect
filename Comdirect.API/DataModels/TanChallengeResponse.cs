namespace Comdirect.API.DataModels;

public class TanChallengeResponse
{
    public string? id { get; set; }
    public string? typ { get; set; }
    public string[] availableTypes { get; set; } = [];
    public string? challenge { get; set; }
    public Link? link { get; set; }
}

public class Link
{
    public string? href { get; set; }
    public string? rel { get; set; }
    public string? method { get; set; }
    public string? type { get; set; }
}