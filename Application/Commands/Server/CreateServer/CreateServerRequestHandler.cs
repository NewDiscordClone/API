using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using MediatR;

namespace Application.Commands.Server.CreateServer
{
    public class CreateServerRequestHandler : IRequestHandler<CreateServerRequest, int>
    {
        private readonly IAppDbContext _context;

        public CreateServerRequestHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateServerRequest request, CancellationToken cancellationToken)
        {
            User user = await _context.Users
                .FindAsync(new object[] { request.UserId },
                cancellationToken: cancellationToken)
                ?? throw new EntityNotFoundException($"User {request.UserId} not found");

            Models.Server server = new()
            {
                Title = request.Title,
                Image = request.Image,
            };
            server.ServerProfiles.Add(new() { User = user, Server = server });

            await _context.Servers.AddAsync(server);
            await _context.SaveChangesAsync();
            return server.Id;
        }
    }
}
