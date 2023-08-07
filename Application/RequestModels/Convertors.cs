using Application.Models;
using Application.RequestModels.GetMessages;
using Application.RequestModels.GetPrivateChats;
using Application.RequestModels.GetServer;
using Application.RequestModels.GetUser;

namespace Application.RequestModels
{
    internal static class Convertors
    {
        public static GetUserDto Convert(User user)
        {
            return new GetUserDto
            {
                Id = user.Id,
                Status = user.Status,
                Username = user.UserName,
                DisplayName = user.DisplayName ?? user.UserName,
                AvatarPath = user.AvatarPath ?? "https://archive.org/download/discordprofilepictures/discordgreen.png",
                TextStatus = user.TextStatus
            };
        }

        public static GetServerDto Convert(Server server)
        {
            List<GetServerProfileDto> serverProfiles = new();
            List<GetRoleDto> roles = new();
            List<GetChannelDto> channels = new();

            server.ServerProfiles.ForEach(sp => serverProfiles.Add(Convert(sp)));
            server.Channels.ForEach(c => channels.Add(Convert(c)));
            server.Roles.ForEach(r => roles.Add(Convert(r)));

            return new GetServerDto()
            {
                Id = server.Id,
                Title = server.Title,
                Image = server.Image,
                ServerProfiles = serverProfiles,
                Roles = roles,
                Channels = channels,
            };
        }

        public static GetServerProfileDto Convert(ServerProfile serverProfile)
        {
            List<GetRoleDto> roles = new();
            
            serverProfile.Roles.ForEach(r => roles.Add(Convert(r)));
            return new GetServerProfileDto()
            {
                Id = serverProfile.Id,
                Roles = roles,
                User = Convert(serverProfile.User),
                DisplayName = serverProfile.DisplayName ?? serverProfile.User.DisplayName ?? serverProfile.User.UserName
            };
        }
        public static GetRoleDto Convert(Role role)
        {
            return new GetRoleDto()
            {
                Id = role.Id,
                Title = role.Name,
                Color = role.Color
            };
        }
        public static GetChannelDto Convert(Channel channel)
        {
            List<GetMessageDto> messages = new();
            
            channel.Messages.ForEach(m => messages.Add(Convert(m)));
            
            return new GetChannelDto()
            {
                Id = channel.Id,
                Title = channel.Title,
                Messages =messages
            };
        }
        public static GetPrivateChatDto Convert(PrivateChat privateChat)
        {
            List<GetMessageDto> messages = new();
            List<GetUserDto> users = new();
            
            privateChat.Messages.ForEach(m => messages.Add(Convert(m)));
            privateChat.Users.ForEach(u => users.Add(Convert(u)));
            
            return new GetPrivateChatDto()
            {
                Id = privateChat.Id,
                Title = privateChat.Title,
                Image = privateChat.Image,
                Users = users,
                Messages = messages
            };
        }
        public static GetMessageDto Convert(Message message)
        {
            List<GetAttachmentDto> attachments = new();
            List<GetReactionDto> reactions = new();

            message.Attachments.ForEach(a => attachments.Add(Convert(a)));
            message.Reactions.ForEach(r => reactions.Add(Convert(r)));
            
            return new GetMessageDto()
            {
                Id = message.Id,
                User = Convert(message.User),
                Text = message.Text,
                Attachments = attachments,
                Reactions = reactions,
                SendTime = message.SendTime,
                ServerProfileDto = null
            };
        }

        public static GetAttachmentDto Convert(Attachment attachment)
        {
            return new GetAttachmentDto()
            {
                Id = attachment.Id,
                Path = attachment.Path,
                Type = attachment.Type,
                IsSpoiler = attachment.IsSpoiler
            };
        }
        
        public static GetReactionDto Convert(Reaction reaction)
        {
            return new GetReactionDto()
            {
                Id = reaction.Id,
                User = Convert(reaction.User),
                Emoji = reaction.Emoji
            };
        }
    }
}