namespace WarOfEmpires.Services {
    public interface IAllianceAuthorizationRequest {
        bool CanInvite { get; set; }
        bool CanManageRoles { get; set; }
        // TODO add can delete chat messages
    }
}