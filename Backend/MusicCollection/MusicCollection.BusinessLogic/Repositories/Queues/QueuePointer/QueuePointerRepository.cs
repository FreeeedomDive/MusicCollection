using SqlRepositoryBase.Core.Repository;

namespace MusicCollection.BusinessLogic.Repositories.Queues.QueuePointer;

public class QueuePointerRepository : IQueuePointerRepository
{
    public QueuePointerRepository(ISqlRepository<QueuePointerStorageElement> sqlRepository)
    {
        this.sqlRepository = sqlRepository;
    }

    public async Task CreateOrUpdateAsync(Guid userId, int position)
    {
        if (await TryReadAsync(userId) != null)
        {
            await sqlRepository.UpdateAsync(userId, x => x.Current = position);
            return;
        }

        await sqlRepository.CreateAsync(
            new QueuePointerStorageElement
            {
                Id = userId,
                Current = position,
            }
        );
    }

    public async Task<int?> TryReadAsync(Guid userId)
    {
        return (await sqlRepository.TryReadAsync(userId))?.Current;
    }

    public async Task DeleteAsync(Guid userId)
    {
        await sqlRepository.DeleteAsync(userId);
    }

    private readonly ISqlRepository<QueuePointerStorageElement> sqlRepository;
}