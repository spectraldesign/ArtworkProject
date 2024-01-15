using Application.DTO.User;
using Application.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Commands.UserCommands
{
    public class UpdateUserCommand : IRequest<IdentityResult>
    {
        public UpdateUserDTO CreateUserDTO { get; set; }
        public UpdateUserCommand(UpdateUserDTO createUserDTO)
        {
            CreateUserDTO = createUserDTO;
        }
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, IdentityResult>
    {
        private readonly IUserRepository _userRepository;
        public UpdateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<IdentityResult> Handle(UpdateUserCommand command, CancellationToken token)
        {
            return await _userRepository.UpdateUserAsync(command.CreateUserDTO);
        }
    }
}
