namespace Cortside.AspNetCore.EntityFramework.Searches {
    public interface ISearch {
        int PageNumber { get; set; }
        int PageSize { get; set; }
        string Sort { get; set; }
    }
}
