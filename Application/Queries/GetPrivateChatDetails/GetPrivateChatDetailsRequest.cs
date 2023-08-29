using Application.Models;
using MediatR;
using MongoDB.Bson;

namespace Application.Queries.GetPrivateChatDetails
{
    public class GetPrivateChatDetailsRequest : IRequest<PrivateChat>
    {
        public string ChatId { get; init; }
    }
}