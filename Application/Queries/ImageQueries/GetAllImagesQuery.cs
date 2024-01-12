using Application.DTO.Image;
using Application.Repositories;
using MediatR;

namespace Application.Queries.ImageQueries
{
    public class GetAllImagesQuery : IRequest<List<GetAllImagesDTO>> { }
    public class GetAllImagesQueryHandler : IRequestHandler<GetAllImagesQuery, List<GetAllImagesDTO>>
    {
        private readonly IImageRepository _imageRepository;
        public GetAllImagesQueryHandler(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }
        public async Task<List<GetAllImagesDTO>> Handle(GetAllImagesQuery request, CancellationToken cancellationToken)
        {
            return await _imageRepository.GetAllImagesAsync();
        }
    }
}
