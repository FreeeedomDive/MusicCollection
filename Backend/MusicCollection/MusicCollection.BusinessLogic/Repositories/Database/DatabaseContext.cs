using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MusicCollection.BusinessLogic.Repositories.Auth;
using MusicCollection.BusinessLogic.Repositories.Files.Nodes;
using MusicCollection.BusinessLogic.Repositories.Files.Roots;
using MusicCollection.BusinessLogic.Repositories.Files.Tags;
using MusicCollection.BusinessLogic.Repositories.Queues.QueueContext;
using MusicCollection.BusinessLogic.Repositories.Queues.QueueList;
using MusicCollection.BusinessLogic.Repositories.Queues.QueuePointer;

namespace MusicCollection.BusinessLogic.Repositories.Database;

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
    public DbSet<UserStorageElement> UsersStorage { get; set; }
    public DbSet<AudioFileTagsStorageElement> TagsStorage { get; set; }
    public DbSet<QueueContextStorageElement> QueueContextStorage { get; set; }
    public DbSet<QueuePointerStorageElement> QueuePointerStorage { get; set; }
    public DbSet<QueueListStorageElement> QueueListStorage { get; set; }
}