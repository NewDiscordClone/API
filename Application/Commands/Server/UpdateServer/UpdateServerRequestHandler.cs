using Application.Interfaces;
using MediatR;
namespace Application.Commands.Server.UpdateServer
{
    public class UpdateServerRequestHandler : IRequestHandler<UpdateServerRequest>
    {
        private readonly IAppDbContext _context;

        public UpdateServerRequestHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task Handle(UpdateServerRequest request, CancellationToken cancellationToken)
        {
            Models.Server server = await _context.FindByIdAsync<Models.Server>
                (request.ServerId, cancellationToken);

            server.Title = request.Title ?? server.Title;
            server.Image = request.Image ?? server.Image;

            _context.Servers.Update(server);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
