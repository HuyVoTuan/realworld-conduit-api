using System.Collections;
using System.Net;

namespace RealWorldConduit.Infrastructure.Common
{
    public class RestException : Exception
    {
        private HttpStatusCode _Code { get; set; }
        public static string STATUS_CODE = "Status_code";
        public RestException(HttpStatusCode code, string message) : base(message)
        {
            _Code = code;
        }

        public override IDictionary Data => new Dictionary<string, HttpStatusCode>()
        {
            {STATUS_CODE, _Code }
        };
    }
}
