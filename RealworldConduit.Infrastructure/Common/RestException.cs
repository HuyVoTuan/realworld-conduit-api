using System.Collections;
using System.Net;

namespace RealWorldConduit.Infrastructure.Common
{
    public class RestException : Exception
    {
        public static string STATUS_CODE = "Status_code";
        public override IDictionary Data => new Dictionary<string, HttpStatusCode>()
        {
            {STATUS_CODE, _Code }
        };

        private HttpStatusCode _Code { get; }
        private IDictionary<string, IEnumerable<string>> _Errors { get; }

        public RestException(HttpStatusCode code, string message) : base(message)
        {
            _Code = code;
        }
        public RestException(HttpStatusCode code, IDictionary<string, IEnumerable<string>> errors)
        {
            _Code = code;
            _Errors = errors;
        }

    }
}
