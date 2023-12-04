namespace RealworldConduit.Infrastructure.Common
{
    public class PagingRequestDTO
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
