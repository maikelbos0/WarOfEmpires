using WarOfEmpires.Database;
using Microsoft.EntityFrameworkCore;

namespace WarOfEmpires.Test.Utilities {
    public sealed class FakeWarContext : WarContext {
        public int CallsToSaveChanges { get; private set; }

        public FakeWarContext() : base(new DbContextOptionsBuilder<WarContext>().UseInMemoryDatabase("WarOfEmpires").Options) {
        }

        public override int SaveChanges() {
            CallsToSaveChanges++;

            return base.SaveChanges();
        }
    }
}