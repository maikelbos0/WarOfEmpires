namespace WarOfEmpires.Services {
    public interface IResourceService {
        string ResolveStaticResource(string relativePath);
        string ResolveUserResource(string relativePath);
    }
}
