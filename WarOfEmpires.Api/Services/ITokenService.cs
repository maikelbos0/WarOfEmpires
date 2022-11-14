namespace WarOfEmpires.Api.Services {
    public interface ITokenService {
        string CreateToken(bool isAdmin);
    }
}