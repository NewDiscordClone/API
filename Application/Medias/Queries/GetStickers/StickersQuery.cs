using MediatR;

namespace Sparkle.Application.Medias.Queries.GetStickers
{
    public record StickersQuery : IRequest<string[]>;
}
