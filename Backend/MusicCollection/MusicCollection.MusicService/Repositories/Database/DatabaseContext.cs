using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MusicCollection.MusicService.Repositories.Files.Nodes;
using MusicCollection.MusicService.Repositories.Files.Roots;
using MusicCollection.MusicService.Repositories.Files.Tags;
using MusicCollection.MusicService.Repositories.Queues.QueueContext;
using MusicCollection.MusicService.Repositories.Queues.QueueList;
using MusicCollection.MusicService.Repositories.Queues.QueuePointer;

namespace MusicCollection.MusicService.Repositories.Database;

public class DatabaseContext : DbContext
{
    public DatabaseOptions Options { get; }

    public DatabaseContext(
        DbContextOptions<DatabaseContext> options,
        IOptions<DatabaseOptions> dbOptionsAccessor
    ) : base(options)
    {
        Options = dbOptionsAccessor.Value;
    }
        
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(Options.ConnectionString);
    }

    public DbSet<NodeStorageElement> NodesStorage { get; set; }
    public DbSet<RootStorageElement> RootsStorage { get; set; }
    public DbSet<AudioFileTagsStorageElement> TagsStorage { get; set; }
    public DbSet<QueueContextStorageElement> QueueContextStorage { get; set; }
    public DbSet<QueuePointerStorageElement> QueuePointerStorage { get; set; }
    public DbSet<QueueListStorageElement> QueueListStorage { get; set; }
}