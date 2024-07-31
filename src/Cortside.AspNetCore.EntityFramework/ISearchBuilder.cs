using System.Linq;

namespace Cortside.AspNetCore.EntityFramework {
    public interface ISearchBuilder<T> {
        IQueryable<T> Build(IQueryable<T> list);
    }
}
