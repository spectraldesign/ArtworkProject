using Application.DTO.User;
using Application.Repositories;
using MediatR;

namespace Application.Commands.UserCommands
{
    public class CreateTokenCommand : IRequest<string>
    {
        public CreateUserDTO loginDTO;
        public CreateTokenCommand(CreateUserDTO loginDTO)
        {
            this.loginDTO = loginDTO;
        }
    }

    public class CreateTokenCommandHandler : IRequestHandler<CreateTokenCommand, string>
    {
        private readonly IUserRepository _userRepository;

        public CreateTokenCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<string> Handle(CreateTokenCommand command, CancellationToken token)
        {
            return await _userRepository.CreateTokenAsync(command.loginDTO);
        }
    }
}
