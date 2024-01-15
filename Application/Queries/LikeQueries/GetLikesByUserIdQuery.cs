using Application.DTO.Like;
using Application.Repositories;
using MediatR;

namespace Application.Queries.LikeQueries
{
    public class GetLikesByUserIdQuery : IRequest<List<LikeDTO>>
    {
        public string UserId { get; set; }
        public GetLikesByUserIdQuery(string userId)
        {
            UserId = userId;
        }
    }
    public class GetLikesByUserIdQueryHandler : IRequestHandler<GetLikesByUserIdQuery, List<LikeDTO>>
    {
        private readonly ILikeRepository _likeRepository;
        public GetLikesByUserIdQueryHandler(ILikeRepository likeRepository)
        {
            _likeRepository = likeRepository;
        }
        public async Task<List<LikeDTO>> Handle(GetLikesByUserIdQuery request, CancellationToken token)
        {
            return await _likeRepository.GetLikesByUserIdAsync(request.UserId);
        }
    }
}
