using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Delo.Interfaces
{
    public interface IRepository<T> where T : class, IEntity, new()
    {
        IQueryable<T> Items { get; }

        T Get(int id);

        Task<T> GetAsync(int id, CancellationToken cancel = default);

        bool CanConnect<T>();
    }
}
