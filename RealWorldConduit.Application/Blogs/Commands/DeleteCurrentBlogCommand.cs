using Microsoft.EntityFrameworkCore;
using RealworldConduit.Infrastructure.Common;
using RealWorldConduit.Infrastructure;
using RealWorldConduit.Infrastructure.Auth;
using RealWorldConduit.Infrastructure.Common;
using System.Net;

namespace RealWorldConduit.Application.Blogs.Commands
{
    public class DeleteCurrentBlogCommand : IRequestWithBaseResponse
    {
        // TODO : Implement Validation Later
        public string Slug { get; init; }

    }

    internal class DeleteBlogCommandHandler : IRequestWithBaseResponseHandler<DeleteCurrentBlogCommand>
    {
        private readonly MainDbContext _dbContext;
        private readonly ICurrentUser _currentUser;

        public DeleteBlogCommandHandler(MainDbContext dbContext, ICurrentUser currentUser)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
        }
        public async Task<BaseResponseDTO> Handle(DeleteCurrentBlogCommand request, CancellationToken cancellationToken)
        {
            var oldBlog = await _dbContext.Blogs
                                .FirstOrDefaultAsync(x => x.Slug.Equals(request.Slug) && x.AuthorId == _currentUser.Id, cancellationToken);

            if (oldBlog is null)
            {
                throw new RestException(HttpStatusCode.NotFound, $"{request.Slug} blog is not found!");
            }

            _dbContext.Blogs.Remove(oldBlog);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new BaseResponseDTO
            {
                Code = HttpStatusCode.NoContent,
                Message = $"{oldBlog.Slug} blog has been successfully deleted",
            };
        }
    }
}
