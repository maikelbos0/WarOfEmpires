using WarOfEmpires.Database;

namespace WarOfEmpires.Repositories {
    public class BaseRepository : IBaseRepository {
        protected readonly IWarContext _context;

        public BaseRepository(IWarContext context) {
            _context = context;
        }

        public void SaveChanges() {
            _context.SaveChanges();
        }
    }
}