using Application.DTO.Comment;
using Application.Repositories;
using MediatR;

namespace Application.Queries.CommentQueries
{
    public class GetCommentsByUserIdQuery : IRequest<List<CommentDTO>>
    {
        public string Id { get; set; }

        public GetCommentsByUserIdQuery(string id)
        {
            Id = id;
        }
    }

    public class GetCommentsByUserIdQueryHandler : IRequestHandler<GetCommentsByUserIdQuery, List<CommentDTO>>
    {
        private readonly ICommentRepository _repository;

        public GetCommentsByUserIdQueryHandler(ICommentRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<CommentDTO>> Handle(GetCommentsByUserIdQuery query, CancellationToken token)
        {
            return await _repository.GetCommentsByImageIdAsync(query.Id);
        }
    }
}
