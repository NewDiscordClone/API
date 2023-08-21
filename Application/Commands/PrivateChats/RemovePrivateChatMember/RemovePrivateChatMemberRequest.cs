﻿using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Commands.PrivateChats.RemovePrivateChatMember
{
    public class RemovePrivateChatMemberRequest : IRequest
    {
        public int ChatId { get; init; }
        public int MemberId { get; init; }
    }
}