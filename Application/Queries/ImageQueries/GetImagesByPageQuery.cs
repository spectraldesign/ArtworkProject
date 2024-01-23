using Application.DTO.Image;
using Application.Repositories;
using MediatR;

namespace Application.Queries.ImageQueries
{
    public class GetImagesByPageQuery : IRequest<GetImageByPageDTO>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public GetImagesByPageQuery(int page, int pageSize)
        {
            Page = page;
            PageSize = pageSize;
        }
    }
    public class GetImagesByPageQueryHandler : IRequestHandler<GetImagesByPageQuery, GetImageByPageDTO>
    {
        private readonly IImageRepository _imageRepository;
        public GetImagesByPageQueryHandler(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }
        public async Task<GetImageByPageDTO> Handle(GetImagesByPageQuery request, CancellationToken cancellationToken)
        {
            return await _imageRepository.GetImagesByPageAsync(request.Page, request.PageSize);
        }
    }
}
