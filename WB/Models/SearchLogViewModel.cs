namespace WB.Models
{
    public class SearchLogViewModel
    {
        public IEnumerable<ScanLog> Logs { get; set; }
        public IEnumerable<Category> List { get; set; }
        public IEnumerable<string> DistinctLines { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? MaterialNumber { get; set; }
        public string? Status { get; set; }
        public string? Supplier { get; set; }
        public string? Line { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int TotalItems { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    }

}
