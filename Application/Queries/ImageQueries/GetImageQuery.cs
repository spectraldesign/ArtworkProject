using Application.DTO.Image;
using Application.Repositories;
using MediatR;

namespace Application.Queries.ImageQueries
{
    public class GetImageQuery : IRequest<ImageDTO>
    {
        public string _imageId { get; set; }
        public GetImageQuery(string imageId)
        {
            _imageId = imageId;
        }
    }

    public class GetImageQueryHandler : IRequestHandler<GetImageQuery, ImageDTO>
    {
        private readonly IImageRepository _imageRepository;
        public GetImageQueryHandler(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }
        public async Task<ImageDTO> Handle(GetImageQuery request, CancellationToken cancellationToken)
        {
            return await _imageRepository.GetImageByIdAsync(request._imageId);
        }
    }
}
