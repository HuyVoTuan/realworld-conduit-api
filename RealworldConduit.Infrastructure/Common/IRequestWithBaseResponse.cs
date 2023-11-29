using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RealWorldConduit.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealworldConduit.Infrastructure.Common
{
    public interface IRequestWithBaseResponse<T> : IRequest<BaseResponse<T>> 
    {

    }

    public interface IRequestWithBaseResponse : IRequest<BaseResponse>
    {

    }
}
