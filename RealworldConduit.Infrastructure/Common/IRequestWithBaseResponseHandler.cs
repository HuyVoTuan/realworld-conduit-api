using MediatR;
using RealWorldConduit.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealworldConduit.Infrastructure.Common
{
    public interface IRequestWithBaseResponseHandler<TRequest, TResponse> : IRequestHandler<TRequest, BaseResponse<TResponse>> where TRequest : IRequestWithBaseResponse<TResponse>
    {
    }

    public interface IRequestWithBaseResponseHandler<TRequest> : IRequestHandler<TRequest, BaseResponse> where TRequest : IRequestWithBaseResponse
    {
    }
}
