using Application.DTO.Comment;
using Application.Repositories;
using MediatR;

namespace Application.Queries.CommentQueries
{
    public class GetCommentsByImageIdQuery : IRequest<List<CommentDTO>>
    {
        public string Id { get; set; }

        public GetCommentsByImageIdQuery(string id)
        {
            Id = id;
        }
    }

    public class GetCommentsByImageIdQueryHandler : IRequestHandler<GetCommentsByImageIdQuery, List<CommentDTO>>
    {
        private readonly ICommentRepository _repository;

        public GetCommentsByImageIdQueryHandler(ICommentRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<CommentDTO>> Handle(GetCommentsByImageIdQuery query, CancellationToken token)
        {
            return await _repository.GetCommentsByImageIdAsync(query.Id);
        }
    }
}
