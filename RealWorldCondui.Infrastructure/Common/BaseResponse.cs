using System.Net;

namespace RealWorldCondui.Infrastructure.Common
{
    public class BaseResponse<T>
    {
        public HttpStatusCode Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

    }

    public class BaseResponse
    {
        public HttpStatusCode Code { get; set; }
        public string Message { get; set; }
    }
}
