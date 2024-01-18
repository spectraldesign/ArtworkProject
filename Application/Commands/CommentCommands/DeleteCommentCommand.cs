using Application.Repositories;
using MediatR;

namespace Application.Commands.CommentCommands
{
    public class DeleteCommentCommand : IRequest<int>
    {
        public string CommentId { get; set; }

        public DeleteCommentCommand(string commentId)
        {
            CommentId = commentId;
        }
    }

    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, int>
    {
        private readonly ICommentRepository _commentRepository;

        public DeleteCommentCommandHandler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<int> Handle(DeleteCommentCommand command, CancellationToken token)
        {
            return await _commentRepository.DeleteCommentCommandAsync(command.CommentId);
        }
    }
}
