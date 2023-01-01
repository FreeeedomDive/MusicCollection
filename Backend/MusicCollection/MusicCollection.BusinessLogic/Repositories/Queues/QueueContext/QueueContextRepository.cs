using SqlRepositoryBase.Core.Repository;

namespace MusicCollection.BusinessLogic.Repositories.Queues.QueueContext;

public class QueueContextRepository : IQueueContextRepository
{
    public QueueContextRepository(ISqlRepository<QueueContextStorageElement> sqlRepository)
    {
        this.sqlRepository = sqlRepository;
    }

    public async Task CreateOrUpdateAsync(Guid userId, Guid contextId)
    {
        if (await TryReadAsync(userId) != null)
        {
            await sqlRepository.UpdateAsync(userId, x => x.ContextId = contextId);
            return;
        }

        await sqlRepository.CreateAsync(new QueueContextStorageElement
        {
            Id = userId,
            ContextId = contextId
        });
    }

    public async Task<Guid?> TryReadAsync(Guid userId)
    {
        return (await sqlRepository.TryReadAsync(userId))?.ContextId;
    }

    private readonly ISqlRepository<QueueContextStorageElement> sqlRepository;
}