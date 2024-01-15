using Application.DTO.User;
using Application.Repositories;
using MediatR;

namespace Application.Queries.UserQueries
{
    public class GetLoggedInUserQuery : IRequest<UserDTO>
    {
        public GetLoggedInUserQuery() { }
    }

    public class GetLoggedInUserQueryHandler : IRequestHandler<GetLoggedInUserQuery, UserDTO>
    {
        private readonly IUserRepository _userRepository;
        public GetLoggedInUserQueryHandler(IUserRepository repo)
        {
            _userRepository = repo;
        }
        public async Task<UserDTO> Handle(GetLoggedInUserQuery request, CancellationToken cancellationToken)
        {
            return await _userRepository.GetLoggedInUserAsync();
        }
    }
}
