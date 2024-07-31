namespace Cortside.AspNetCore.EntityFramework.Searches {
    public class Search : ISearch {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 30;
        public string Sort { get; set; } = "-CreatedDate";
    }
}
