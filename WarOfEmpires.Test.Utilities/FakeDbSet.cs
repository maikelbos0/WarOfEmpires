using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace WarOfEmpires.Test.Utilities {
    public sealed class FakeDbSet<TEntity> : DbSet<TEntity>, IEnumerable<TEntity>, IQueryable where TEntity : class {
        private readonly List<TEntity> data = new List<TEntity>();

        public override EntityEntry<TEntity> Add(TEntity entity) {
            data.Add(entity);

            return null;
        }

        IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator() {
            return data.GetEnumerator();
        }

        IQueryProvider IQueryable.Provider => data.AsQueryable().Provider;

        Expression IQueryable.Expression => data.AsQueryable().Expression;
    }
}