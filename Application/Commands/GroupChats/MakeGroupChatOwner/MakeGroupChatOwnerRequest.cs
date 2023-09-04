﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MediatR;
using MongoDB.Bson;

namespace Application.Commands.GroupChats.MakeGroupChatOwner
{
    public class MakeGroupChatOwnerRequest : IRequest
    {
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ChatId { get; init; }
        
        [Required]
        [DefaultValue(1)]
        public int MemberId { get; init; }
    }
}