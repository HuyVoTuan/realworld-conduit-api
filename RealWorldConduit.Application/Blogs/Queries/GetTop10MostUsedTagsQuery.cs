using Microsoft.EntityFrameworkCore;
using RealworldConduit.Infrastructure.Common;
using RealWorldConduit.Application.Blogs.DTOs;
using RealWorldConduit.Infrastructure;
using RealWorldConduit.Infrastructure.Common;
using System.Net;



namespace RealWorldConduit.Application.Blogs.Queries
{
    public class GetTop10MostUsedTagsQuery : IRequestWithBaseResponse<TagDTO> { };
    internal class GetTop10MostUsedTagsQueryHandler : IRequestWithBaseResponseHandler<GetTop10MostUsedTagsQuery, TagDTO>
    {
        private readonly MainDbContext _dbContext;

        public GetTop10MostUsedTagsQueryHandler(MainDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BaseResponseDTO<TagDTO>> Handle(GetTop10MostUsedTagsQuery request, CancellationToken cancellationToken)
        {
            var tags = await _dbContext.BlogsTag.AsNoTracking()
                                                .GroupBy(x => x.Tag.Name)
                                                .OrderByDescending(group => group.Count())
                                                .Take(10)
                                                .Select(x => x.Key)
                                                .ToListAsync(cancellationToken);

            var tagsDTO = new TagDTO
            {
                Tags = tags
            };

            return new BaseResponseDTO<TagDTO>
            {
                Code = HttpStatusCode.OK,
                Message = "Successfully get top 10 most used tags",
                Data = tagsDTO
            };
        }
    }
}