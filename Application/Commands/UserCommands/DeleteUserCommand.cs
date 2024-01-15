using Application.Repositories;
using MediatR;

namespace Application.Commands.UserCommands
{
    public class DeleteUserCommand : IRequest<int>
    {
        public string Id { get; set; }
        public DeleteUserCommand(string id)
        {
            Id = id;
        }
    }

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, int>
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<int> Handle(DeleteUserCommand deleteUserCommand, CancellationToken token)
        {
            return await _userRepository.DeleteUserAsync(deleteUserCommand.Id);
        }
    }
}
