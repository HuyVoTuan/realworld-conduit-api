using System.Net;

namespace RealWorldConduit.Infrastructure.Common
{
    public class BaseResponse<T>
    {
        public HttpStatusCode Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public BaseResponse()
        {
        }
        public BaseResponse(T data)
        {
            Data = data;
        }
    }

    public class BaseResponse
    {
        public HttpStatusCode Code { get; set; }
        public string Message { get; set; }
    }
}
