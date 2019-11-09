using WarOfEmpires.Database;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Utilities.Container;
using System;
using System.Linq;

namespace WarOfEmpires.Repositories.Security {
    [InterfaceInjectable]
    public sealed class UserRepository : IUserRepository {
        private readonly IWarContext _context;

        public UserRepository(IWarContext context) {
            _context = context;
        }

        public User TryGetByEmail(string email) {
            return _context.Users.SingleOrDefault(u => u.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));
        }

        public User GetActiveByEmail(string email) {
            return GetByEmail(email, UserStatus.Active);
        }

        public User GetByEmail(string email, UserStatus status) {
            return _context.Users.Single(u => u.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase) && u.Status == status);
        }

        public void Add(User user) {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Update() {
            _context.SaveChanges();
        }
    }
}