namespace Cortside.AspNetCore.EntityFramework {
    public interface ISearch {
        int PageNumber { get; set; }
        int PageSize { get; set; }
        string Sort { get; set; }
    }
}
