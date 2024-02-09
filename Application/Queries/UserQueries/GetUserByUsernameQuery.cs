using Application.DTO.User;
using Application.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.UserQueries
{
    public class GetUserByUsernameQuery : IRequest<UserDTO>
    {
        public string UserName { get; set; }
        public GetUserByUsernameQuery(string username) 
        {
            UserName = username;
        }
    }

    public class GetUserByUsernameQueryHandler : IRequestHandler<GetUserByUsernameQuery, UserDTO>
    {
        private readonly IUserRepository _userRepository;
        public GetUserByUsernameQueryHandler(IUserRepository repo)
        {
            _userRepository = repo;
        }
        public async Task<UserDTO> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
        {
            return await _userRepository.GetUserByUsername(request.UserName);
        }
    }
}
