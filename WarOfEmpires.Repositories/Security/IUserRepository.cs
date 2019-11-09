using WarOfEmpires.Domain.Security;

namespace WarOfEmpires.Repositories.Security {
    public interface IUserRepository {
        User TryGetByEmail(string email);
        User GetActiveByEmail(string email);
        User GetByEmail(string email, UserStatus status);
        void Add(User user);
        void Update();
    }
}