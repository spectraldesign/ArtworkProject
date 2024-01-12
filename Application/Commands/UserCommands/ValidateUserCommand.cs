using Application.DTO.User;
using Application.Repositories;
using MediatR;

namespace Application.Commands.UserCommands
{
    public class ValidateUserCommand : IRequest<bool>
    {
        public readonly CreateUserDTO _validateUserDTO;

        public ValidateUserCommand(CreateUserDTO validateUserDTO)
        {
            _validateUserDTO = validateUserDTO;
        }
    }

    public class ValidateUserCommandHandler : IRequestHandler<ValidateUserCommand, bool>
    {
        public readonly IUserRepository _userRepository;

        public ValidateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(ValidateUserCommand command, CancellationToken token)
        {
            return await _userRepository.ValidateUserAsync(command._validateUserDTO);
        }
    }
}
