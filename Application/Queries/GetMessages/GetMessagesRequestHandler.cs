﻿using Application.Interfaces;
using Application.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Application.Queries.GetMessages
{
    public class GetMessagesRequestHandler : RequestHandlerBase, IRequestHandler<GetMessagesRequest, List<Message>>
    {
        private const int _pageSize = 25;

        public async Task<List<Message>> Handle(GetMessagesRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            return await Context.GetMessagesAsync(request.ChatId, request.MessagesCount, _pageSize);
        }

        public GetMessagesRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) :
            base(context, userProvider, mapper)
        {
        }
    }
}