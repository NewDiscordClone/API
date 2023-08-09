using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using MediatR;

namespace Application.Queries.GetServerDetails
{
    public class GetServerDetailsRequestHandler : IRequestHandler<GetServerDetailsRequest, ServerDetailsDto>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public GetServerDetailsRequestHandler(IAppDbContext appDbContext, IMapper mapper)
        {
            _context = appDbContext;
            _mapper = mapper;
        }

        public async Task<ServerDetailsDto> Handle(GetServerDetailsRequest request,
            CancellationToken cancellationToken)
        {
            Server server = await _context.Servers.FindAsync(new object[] { request.ServerId }, cancellationToken)
                ?? throw new EntityNotFoundException($"Server {request.ServerId} not found");

            return _mapper.Map<ServerDetailsDto>(server);
        }
    }
}
