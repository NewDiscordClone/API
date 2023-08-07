using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.RequestModels.GetUser
{
    public class GetUserRequestHandler : IRequestHandler<GetUserRequest, GetUserDto>
    {
        private readonly IAppDbContext _appDbContext;

        public GetUserRequestHandler(IAppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }

        public async Task<GetUserDto> Handle(GetUserRequest request, CancellationToken cancellationToken)
        {
            var user = await _appDbContext.Users.FindAsync(new object[] { request.UserId }, cancellationToken: cancellationToken);
            if (user == null) throw new NoSuchUserException();
            return Convertors.Convert(user);
        }
    }
}