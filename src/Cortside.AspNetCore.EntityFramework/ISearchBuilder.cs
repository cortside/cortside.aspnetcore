using System.Linq;

namespace LiveTula.ConnectApi.Data.Searches {
    public interface ISearchBuilder<T> {
        IQueryable<T> Build(IQueryable<T> list);
    }
}
