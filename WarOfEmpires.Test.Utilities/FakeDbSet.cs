using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace WarOfEmpires.Test.Utilities {
    public sealed class FakeDbSet<TEntity> : IDbSet<TEntity> where TEntity : class {
        private readonly List<TEntity> _data;

        public FakeDbSet() {
            _data = new List<TEntity>();
        }

        public FakeDbSet(params TEntity[] entities) {
            _data = new List<TEntity>(entities);
        }

        public IEnumerator<TEntity> GetEnumerator() {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return _data.GetEnumerator();
        }

        public Expression Expression {
            get { return Expression.Constant(_data.AsQueryable()); }
        }

        public Type ElementType {
            get { return typeof(TEntity); }
        }

        public IQueryProvider Provider {
            get { return _data.AsQueryable().Provider; }
        }

        public TEntity Find(params object[] keyValues) {
            throw new NotImplementedException();
        }

        public TEntity Add(TEntity entity) {
            _data.Add(entity);
            return entity;
        }

        public TEntity Remove(TEntity entity) {
            _data.Remove(entity);
            return entity;
        }

        public TEntity Attach(TEntity entity) {
            _data.Add(entity);
            return entity;
        }

        public TEntity Create() {
            return Activator.CreateInstance<TEntity>();
        }

        public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, TEntity {
            return Activator.CreateInstance<TDerivedEntity>();
        }

        public ObservableCollection<TEntity> Local {
            get { return new ObservableCollection<TEntity>(_data); }
        }
    }
}
