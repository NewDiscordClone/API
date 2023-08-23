﻿using Application.Models;
using MediatR;
using MongoDB.Bson;

namespace Application.Commands.Messages.AddReaction
{
    public class AddReactionRequest : IRequest<Reaction>
    {
        public ObjectId MessageId { get; init; }
        public string Emoji { get; init; }
    }
}