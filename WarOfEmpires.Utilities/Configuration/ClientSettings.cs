namespace WarOfEmpires.Utilities.Configuration;

public sealed class ClientSettings {
    public string BaseUrl { get; set; } = null!;
    public string TokenAudience { get; set; } = null!;
    public string TokenIssuer { get; set; } = null!;
    public int TokenExpirationTimeInMinutes { get; set; }
}
