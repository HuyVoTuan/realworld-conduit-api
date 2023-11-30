using MediatR;
using RealWorldConduit.Infrastructure.Common;

namespace RealworldConduit.Infrastructure.Common
{
    public interface IRequestWithBaseResponse<T> : IRequest<BaseResponse<T>>
    {

    }

    public interface IRequestWithBaseResponse : IRequest<BaseResponse>
    {

    }
}
