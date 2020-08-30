using WarOfEmpires.Domain.Security;

namespace WarOfEmpires.Repositories.Security {
    public interface IUserRepository : IBaseRepository {
        User TryGetByEmail(string email);
        User GetActiveByEmail(string email);
        void Add(User user);
    }
}