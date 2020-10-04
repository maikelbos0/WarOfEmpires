using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace WarOfEmpires.Database.Orphans {
    public sealed class OrphanedEntityEnumerable<TOrphan> : IEnumerable<TOrphan> {
        private readonly IEnumerable<object> _entities;
        private IEnumerable<TOrphan> _candidates;

        public OrphanedEntityEnumerable(IEnumerable<DbEntityEntry> entries) {
            _entities = entries.Select(e => e.Entity);
            _candidates = _entities.OfType<TOrphan>();
        }

        public IEnumerator<TOrphan> GetEnumerator() {
            return _candidates.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return _candidates.GetEnumerator();
        }

        public OrphanedEntityEnumerable<TOrphan> RemoveChildren<TParent>(Func<TParent, IEnumerable<TOrphan>> childLocator) {
            var children = _entities.OfType<TParent>().SelectMany(p => childLocator(p)).ToHashSet();

            _candidates = _candidates.Where(c => !children.Contains(c));

            return this;
        }
    }
}