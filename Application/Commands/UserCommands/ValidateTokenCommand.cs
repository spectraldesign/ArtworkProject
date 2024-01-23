using Application.DTO.User;
using Application.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UserCommands
{
    public class ValidateTokenCommand(VerifyTokenDTO verifyTokenDTO) : IRequest<bool>
    {
        public readonly VerifyTokenDTO VerifyTokenDTO = verifyTokenDTO;
    }

    public class ValidateTokenCommandHandler : IRequestHandler<ValidateTokenCommand, bool>
    {
        public readonly IUserRepository _userRepository;

        public ValidateTokenCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(ValidateTokenCommand request, CancellationToken cancellationToken)
        {
            return await _userRepository.CheckTokenValidity(request.VerifyTokenDTO);
        }
    }
}
