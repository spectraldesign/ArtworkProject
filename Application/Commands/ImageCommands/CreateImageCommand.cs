using Application.DTO.Image;
using Application.Repositories;
using MediatR;

namespace Application.Commands.Image
{
    public class CreateImageCommand : IRequest<int>
    {
        public readonly CreateImageDTO _createImageDTO;

        public CreateImageCommand(CreateImageDTO createImageDTO)
        {
            _createImageDTO = createImageDTO;
        }
    }

    public class CreateImageCommandHandler : IRequestHandler<CreateImageCommand, int>
    {
        private readonly IImageRepository _imageRepository;

        public CreateImageCommandHandler(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        public async Task<int> Handle(CreateImageCommand command, CancellationToken token)
        {
            return await _imageRepository.CreateImageAsync(command._createImageDTO);
        }
    }
}


