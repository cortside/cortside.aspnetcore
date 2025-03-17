namespace Cortside.AspNetCore.Common.Dtos {
    public class SearchDto {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 30;
        public string Sort { get; set; }
    }
}
