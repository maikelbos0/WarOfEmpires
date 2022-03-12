using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace WarOfEmpires.Test.Utilities {
    public sealed class FakeDbSet<TEntity> : DbSet<TEntity>, IEnumerable<TEntity>, IQueryable where TEntity : class {
        private readonly List<TEntity> data = new();

        public override EntityEntry<TEntity> Add(TEntity entity) {
            data.Add(entity);

            return null;
        }

        public override EntityEntry<TEntity> Remove(TEntity entity) {
            data.Remove(entity);

            return null;
        }

        public override void RemoveRange(IEnumerable<TEntity> entities) {
            foreach (var entity in entities) {
                Remove(entity);
            }
        }

        IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator() {
            return data.GetEnumerator();
        }

        IQueryProvider IQueryable.Provider => data.AsQueryable().Provider;

        Expression IQueryable.Expression => data.AsQueryable().Expression;

        public override IEntityType EntityType => throw new NotImplementedException();
    }
}
