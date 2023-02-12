using MusicCollection.Api.Dto.Exceptions.Api;

namespace MusicCollection.Api.Dto.Exceptions.Files;

public class RootNotFoundException : MusicCollectionApiNotFoundException
{
    public RootNotFoundException(Guid id) : base($"Root {id} not found")
    {
    }
}