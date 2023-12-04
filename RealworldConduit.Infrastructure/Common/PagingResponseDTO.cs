namespace RealworldConduit.Infrastructure.Common
{
    public class PagingResponseDTO<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public List<T> Data { get; set; }
    }
}
