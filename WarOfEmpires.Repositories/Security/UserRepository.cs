using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.Repositories.Security {
    [InterfaceInjectable]
    public sealed class UserRepository : BaseRepository, IUserRepository {
        public UserRepository(IWarContext context) : base(context) { }

        public User TryGetByEmail(string email) {
            return _context.Users.SingleOrDefault(u => EmailComparisonService.Equals(u.Email, email));
        }

        public User GetActiveByEmail(string email) {
            return _context.Users.Single(u => EmailComparisonService.Equals(u.Email, email) && u.Status == UserStatus.Active);
        }

        public void Add(User user) {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
    }
}