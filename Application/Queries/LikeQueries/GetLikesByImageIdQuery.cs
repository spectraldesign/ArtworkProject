using Application.DTO.Like;
using Application.Repositories;
using MediatR;

namespace Application.Queries.LikeQueries
{
    public class GetLikesByImageIdQuery : IRequest<List<LikeDTO>>
    {
        public string Id { get; set; }

        public GetLikesByImageIdQuery(string id)
        {
            Id = id;
        }
    }

    public class GetLikesByPollQueryHandler : IRequestHandler<GetLikesByImageIdQuery, List<LikeDTO>>
    {
        private readonly ILikeRepository _voteRepository;

        public GetLikesByPollQueryHandler(ILikeRepository voteRepository)
        {
            _voteRepository = voteRepository;
        }

        public async Task<List<LikeDTO>> Handle(GetLikesByImageIdQuery query, CancellationToken token)
        {
            return await _voteRepository.GetLikesByImageIdAsync(query.Id);
        }
    }
}
