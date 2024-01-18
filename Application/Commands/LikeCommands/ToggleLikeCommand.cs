using Application.Repositories;
using MediatR;

namespace Application.Commands.LikeCommands
{
    public class ToggleLikeCommand : IRequest<int>
    {
        public string Id { get; set; }
        public ToggleLikeCommand(string id)
        {
            Id = id;
        }
    }
    public class ToggleLikeCommandHandler : IRequestHandler<ToggleLikeCommand, int>
    {
        private readonly ILikeRepository _likeRepository;
        public ToggleLikeCommandHandler(ILikeRepository likeRepository)
        {
            _likeRepository = likeRepository;
        }
        public async Task<int> Handle(ToggleLikeCommand command, CancellationToken token)
        {
            return await _likeRepository.ToggleLikeAsync(command.Id);
        }
    }
}
