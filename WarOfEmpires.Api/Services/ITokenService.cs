namespace WarOfEmpires.Api.Services {
    public interface ITokenService {
        string CreateToken(string subject, bool isAdmin);
    }
}