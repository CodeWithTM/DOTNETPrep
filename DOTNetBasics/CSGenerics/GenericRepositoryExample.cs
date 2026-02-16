using System.Linq.Expressions;

namespace CSGenerics
{
    internal class GenericRepositoryExample
    {

        public async void Main4()
        {
            QueryOptions<Customer> options = new QueryOptions<Customer>();

            options.Filter = customer => customer.Name.StartsWith("A");

            options.Skip = 1;

            options.OrderBy = customers => customers.OrderBy(c => c.Name);

            options.Includes = new List<Expression<Func<Customer, object>>>
            {
                customer => customer.Orders
            };

            Repository<Customer> repository = new Repository<Customer>(new DbContext());

            await repository.GetAsync(options);

            Customer? c1 = await repository.GetByIdAsync(1);

            /* or in short
            QueryOptions<Customer> queryOptions = new QueryOptions<Customer>()
            {
                Filter = customer => customer.Name.StartsWith("A"),
                OrderBy = customer => customer.OrderBy(c => c.Name),

                Includes = new List<Expression<Func<Customer, object>>>
                {
                    customer => customer.Orders
                }
            };
            */

        }
    }

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public List<Order> Orders { get; set; } = new();
    }

    public class Order
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;
    }

    public class QueryOptions<T> where T : class
    {
        public Expression<Func<T, bool>>? Filter { get; set; }

        public Func<IQueryable<T>, IOrderedQueryable<T>>? OrderBy { get; set; }

        public List<Expression<Func<T, object>>>? Includes { get; set; }

        public int? Skip { get; set; }
    }

    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);

        Task<List<T>> GetAsync(QueryOptions<T>? options = null);
    }

    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T>? _dbSet;

        public Repository(DbContext context)
        {
            _context = context;
            //_dbSet = _context.Set<T>();
        }
        public async Task<T?> GetByIdAsync(int id)
        {
            return await Task.FromResult<T?>(null); // Placeholder implementation
        }

        public async Task<List<T>> GetAsync(QueryOptions<T>? options = null)
        {
            throw new NotImplementedException();
        }
    }

    public class DbContext
    {
    }

    public class DbSet<T> where T : class
    {
    }
}
