using Application.DTO.Comment;
using Application.Repositories;
using MediatR;

namespace Application.Commands.CommentCommands
{
    public class DeleteCommentCommand : IRequest<CommentDTO>
    {
        public string CommentId { get; set; }

        public DeleteCommentCommand(string commentId)
        {
            CommentId = commentId;
        }
    }

    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, CommentDTO>
    {
        private readonly ICommentRepository _commentRepository;

        public DeleteCommentCommandHandler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<CommentDTO> Handle(DeleteCommentCommand command, CancellationToken token)
        {
            return await _commentRepository.DeleteCommentCommandAsync(command.CommentId);
        }
    }
}
