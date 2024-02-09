using Application.DTO.Comment;
using Application.Repositories;
using MediatR;

namespace Application.Commands.CommentCommands
{
    public class CreateCommentCommand : IRequest<CommentDTO>
    {
        public CreateCommentDTO CreateCommentDTO { get; set; }

        public CreateCommentCommand(CreateCommentDTO createCommentDTO)
        {
            CreateCommentDTO = createCommentDTO;
        }
    }

    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, CommentDTO>
    {
        private readonly ICommentRepository _commentRepository;

        public CreateCommentCommandHandler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<CommentDTO> Handle(CreateCommentCommand command, CancellationToken token)
        {
            return await _commentRepository.CreateCommentCommandAsync(command.CreateCommentDTO);
        }
    }
}
