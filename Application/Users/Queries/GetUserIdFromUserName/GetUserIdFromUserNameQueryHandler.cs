using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;

namespace Sparkle.Application.Users.Queries.GetUserIdFromUserName
{
    public class GetUserIdFromUserNameQueryHandler : RequestHandlerBase,
        IRequestHandler<GetUserIdFromUserNameQuery, Guid>
    {
        public GetUserIdFromUserNameQueryHandler(IAppDbContext context) : base(context)
        {
        }

        public async Task<Guid> Handle(GetUserIdFromUserNameQuery request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            return (await Context.SqlUsers
                       .FilterAsync(u => u.UserName == request.UserName))
                   .FirstOrDefault()?.Id 
                   ??
                   throw new EntityNotFoundException($"There is no User with {request.UserName} user name");
        }
    }
}