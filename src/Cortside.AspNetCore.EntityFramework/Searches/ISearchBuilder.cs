using System.Linq;

namespace Cortside.AspNetCore.EntityFramework.Searches {
    public interface ISearchBuilder<T> {
        IQueryable<T> Build(IQueryable<T> list);
    }
}
