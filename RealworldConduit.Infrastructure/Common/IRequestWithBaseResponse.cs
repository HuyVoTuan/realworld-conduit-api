using MediatR;
using RealWorldConduit.Infrastructure.Common;

namespace RealworldConduit.Infrastructure.Common
{
    public interface IRequestWithBaseResponse<T> : IRequest<BaseResponseDTO<T>>
    {

    }

    public interface IRequestWithBaseResponse : IRequest<BaseResponse>
    {

    }
}
