using WarOfEmpires.Database;

namespace WarOfEmpires.Repositories {
    public class BaseRepository : IBaseRepository {
        protected readonly ILazyWarContext _context;

        public BaseRepository(ILazyWarContext context) {
            _context = context;
        }

        public void SaveChanges() {
            _context.SaveChanges();
        }
    }
}