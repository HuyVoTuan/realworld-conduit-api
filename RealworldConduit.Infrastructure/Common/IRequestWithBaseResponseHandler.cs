using MediatR;
using RealWorldConduit.Infrastructure.Common;

namespace RealworldConduit.Infrastructure.Common
{
    public interface IRequestWithBaseResponseHandler<TRequest, TResponse> : IRequestHandler<TRequest, BaseResponse<TResponse>> where TRequest : IRequestWithBaseResponse<TResponse>
    {
    }

    public interface IRequestWithBaseResponseHandler<TRequest> : IRequestHandler<TRequest, BaseResponse> where TRequest : IRequestWithBaseResponse
    {
    }
}
