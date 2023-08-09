using Application.Interfaces;
using MediatR;

namespace Application.Commands.Server.DeleteServer
{
    public class DeleteServerRequestHandler : IRequestHandler<DeleteServerRequest>
    {
        private readonly IAppDbContext _context;

        public DeleteServerRequestHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteServerRequest request, CancellationToken cancellationToken)
        {
            Models.Server server = await _context.FindByIdAsync<Models.Server>
                (request.Id, cancellationToken);

            _context.Servers.Remove(server);
            await _context.SaveChangesAsync();
        }
    }
}
