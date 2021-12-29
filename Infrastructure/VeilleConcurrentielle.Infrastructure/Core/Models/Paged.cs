namespace VeilleConcurrentielle.Infrastructure.Core.Models
{
    public class Paged<T> where T:class
    {
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public List<T> Items { get; set; }
    }
}
