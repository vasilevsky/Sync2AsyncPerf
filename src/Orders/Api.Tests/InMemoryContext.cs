using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using API.Data;

namespace API.Tests
{
    /// <summary>
    /// Implementes factory for in-memory context.
    /// </summary>
    public class InMemoryContextFactory : IReadOnlyContextFactory
    {
        private readonly IReadOnlyContext context;

        public InMemoryContextFactory(IReadOnlyContext context)
        {
            this.context = context;
        }

        public IReadOnlyContext CreateContext()
        {
            return context;
        }
    }

    /// <summary>
    /// Implements in-memory database context used for test perposes.
    /// </summary>
    public class InMemoryContext : IReadOnlyContext
    {
        private TestDbSet<Customer> customers = new TestDbSet<Customer>();

        public DbSet<Customer> Customers
        {
            get { return customers; }
        }

        public void AddCustomer(Customer customer)
        {
            customers.Add(customer);
        }

        public void Dispose()
        {
            customers = new TestDbSet<Customer>();
        }

        #region DbSet test double implementation
        private class TestDbSet<TEntity> : DbSet<TEntity>, IQueryable, IEnumerable<TEntity>, IDbAsyncEnumerable<TEntity>
         where TEntity : class
        {
            ObservableCollection<TEntity> data;
            IQueryable query;

            public TestDbSet()
            {
                data = new ObservableCollection<TEntity>();
                query = data.AsQueryable();
            }

            public IList<TEntity> Data
            {
                get { return data.ToList(); }
            }

            public override TEntity Add(TEntity item)
            {
                data.Add(item);
                return item;
            }

            public override TEntity Remove(TEntity item)
            {
                data.Remove(item);
                return item;
            }

            public override TEntity Attach(TEntity item)
            {
                data.Add(item);
                return item;
            }

            public override TEntity Create()
            {
                return Activator.CreateInstance<TEntity>();
            }

            public override TDerivedEntity Create<TDerivedEntity>()
            {
                return Activator.CreateInstance<TDerivedEntity>();
            }

            public override ObservableCollection<TEntity> Local
            {
                get { return data; }
            }

            Type IQueryable.ElementType
            {
                get { return query.ElementType; }
            }

            Expression IQueryable.Expression
            {
                get { return query.Expression; }
            }

            IQueryProvider IQueryable.Provider
            {
                get { return new TestDbAsyncQueryProvider<TEntity>(query.Provider); }
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return data.GetEnumerator();
            }

            IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
            {
                return data.GetEnumerator();
            }

            IDbAsyncEnumerator<TEntity> IDbAsyncEnumerable<TEntity>.GetAsyncEnumerator()
            {
                return new TestDbAsyncEnumerator<TEntity>(data.GetEnumerator());
            }
        }

        internal class TestDbAsyncQueryProvider<TEntity> : IDbAsyncQueryProvider
        {
            private readonly IQueryProvider _inner;

            internal TestDbAsyncQueryProvider(IQueryProvider inner)
            {
                _inner = inner;
            }

            public IQueryable CreateQuery(Expression expression)
            {
                return new TestDbAsyncEnumerable<TEntity>(expression);
            }

            public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
            {
                return new TestDbAsyncEnumerable<TElement>(expression);
            }

            public object Execute(Expression expression)
            {
                return _inner.Execute(expression);
            }

            public TResult Execute<TResult>(Expression expression)
            {
                return _inner.Execute<TResult>(expression);
            }

            public Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken)
            {
                return Task.FromResult(Execute(expression));
            }

            public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
            {
                return Task.FromResult(Execute<TResult>(expression));
            }
        }

        internal class TestDbAsyncEnumerable<T> : EnumerableQuery<T>, IDbAsyncEnumerable<T>, IQueryable<T>
        {
            public TestDbAsyncEnumerable(IEnumerable<T> enumerable)
                : base(enumerable)
            { }

            public TestDbAsyncEnumerable(Expression expression)
                : base(expression)
            { }

            public IDbAsyncEnumerator<T> GetAsyncEnumerator()
            {
                return new TestDbAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
            }

            IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
            {
                return GetAsyncEnumerator();
            }

            IQueryProvider IQueryable.Provider
            {
                get { return new TestDbAsyncQueryProvider<T>(this); }
            }
        }

        internal class TestDbAsyncEnumerator<T> : IDbAsyncEnumerator<T>
        {
            private readonly IEnumerator<T> _inner;

            public TestDbAsyncEnumerator(IEnumerator<T> inner)
            {
                _inner = inner;
            }

            public void Dispose()
            {
                _inner.Dispose();
            }

            public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
            {
                return Task.FromResult(_inner.MoveNext());
            }

            public T Current
            {
                get { return _inner.Current; }
            }

            object IDbAsyncEnumerator.Current
            {
                get { return Current; }
            }
        }
        #endregion
    }
}
