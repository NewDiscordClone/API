using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.DataAccess.Repositories
{
    public class RelationshipRepository : BaseSqlRepository<Relationship, (Guid Active, Guid Passive)>, IRelationshipRepository
    {
        public RelationshipRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<Relationship> AddAsync(Relationship entity, CancellationToken cancellationToken = default)
        {
            {
                // Проверка наличия "перевернутой" записи
                Relationship? reverseRelationship = await
                    FindOrDefaultAsync((entity.Passive, entity.Active), cancellationToken);

                if (reverseRelationship != null)
                {
                    // "Перевернутая" запись уже существует, выполните необходимую логику обновления
                    reverseRelationship.RelationshipType = entity.RelationshipType; // Например, обновляем тип взаимоотношений
                    DbSet.Entry(reverseRelationship).State = EntityState.Modified; // Помечаем запись как измененную
                    await Context.SaveChangesAsync(cancellationToken); // Сохраняем изменения

                    // Возвращаем обновленную запись (опционально)
                    return reverseRelationship;
                }
                else
                {
                    // "Перевернутая" запись не существует, добавляем новую запись
                    Relationship addedRelationship = await base.AddAsync(entity, cancellationToken);
                    return addedRelationship;
                }
            }
        }

        public override async Task<Relationship> FindAsync((Guid Active, Guid Passive) id, CancellationToken cancellationToken = default)
        {
            return await FindOrDefaultAsync(id, cancellationToken)
                ?? throw new EntityNotFoundException("Relationship not found", id);
        }

        public async Task<Relationship?> FindOrDefaultAsync((Guid Active, Guid Passive) id,
            CancellationToken cancellationToken = default)
        {
            Relationship? relationship = await DbSet.FindAsync(new object?[] { id.Active, id.Passive }, cancellationToken: cancellationToken);
            return relationship ?? await DbSet.FindAsync(new object?[] { id.Passive, id.Active }, cancellationToken: cancellationToken);
        }

    }
}
