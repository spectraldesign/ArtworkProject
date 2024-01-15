using Application.Repositories;
using MediatR;

namespace Application.Commands.ImageCommands
{
    public class DeleteImageCommand : IRequest<int>
    {
        public readonly string _id;
        public DeleteImageCommand(string id)
        {
            _id = id;
        }
    }

    public class DeleteImageCommandHandler : IRequestHandler<DeleteImageCommand, int>
    {
        public IImageRepository _imageRepository;
        public DeleteImageCommandHandler(IImageRepository repo)
        {
            _imageRepository = repo;
        }

        public async Task<int> Handle(DeleteImageCommand command, CancellationToken token)
        {
            return await _imageRepository.DeleteImageAsync(command._id);
        }
    }
}
