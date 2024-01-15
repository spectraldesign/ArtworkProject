using Application.Repositories;
using MediatR;

namespace Application.Commands.LikeCommands
{
    public class CreateLikeCommand : IRequest<int>
    {
        public string Id { get; set; }
        public CreateLikeCommand(string id)
        {
            Id = id;
        }
    }
    public class CreateLikeCommandHandler : IRequestHandler<CreateLikeCommand, int>
    {
        private readonly ILikeRepository _likeRepository;
        public CreateLikeCommandHandler(ILikeRepository likeRepository)
        {
            _likeRepository = likeRepository;
        }
        public async Task<int> Handle(CreateLikeCommand command, CancellationToken token)
        {
            return await _likeRepository.CreateLikeAsync(command.Id);
        }
    }
}
