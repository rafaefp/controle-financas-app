namespace Fina.Core.Requests
{
    public abstract class PagedResponse : Request
    {
        public int PageSize { get; set; } = Configuration.DefaultPageSize;
        public int PageNumber { get; set; } = Configuration.DefaultPageNumber;
    }
}
