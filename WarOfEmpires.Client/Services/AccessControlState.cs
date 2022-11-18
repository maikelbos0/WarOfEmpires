namespace WarOfEmpires.Client.Services;

public sealed class AccessControlState {
    public bool IsAuthenticated { get; private set; }
    public string? DisplayName { get; private set; }
    public bool IsAdmin { get; private set; }

    public AccessControlState(string? displayName = null, bool isAuthenticated = false, bool isAdmin = false) {
        DisplayName = displayName;
        IsAuthenticated = isAuthenticated;
        IsAdmin = isAdmin;
    }
}
