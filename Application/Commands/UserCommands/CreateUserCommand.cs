using Application.DTO.User;
using Application.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Commands.UserCommands
{
    public class CreateUserCommand : IRequest<IdentityResult>
    {
        public readonly CreateUserDTO _updateUserDTO;

        public CreateUserCommand(CreateUserDTO createUserDTO)
        {
            _updateUserDTO = createUserDTO;
        }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, IdentityResult>
    {
        private readonly IUserRepository _userRepository;
        public CreateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IdentityResult> Handle(CreateUserCommand command, CancellationToken token)
        {
            return await _userRepository.CreateUserAsync(command._updateUserDTO);
        }
    }
}
