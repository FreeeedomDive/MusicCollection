using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MusicCollection.BusinessLogic.Repositories.Auth;
using MusicCollection.BusinessLogic.Repositories.Files;

namespace MusicCollection.BusinessLogic.Repositories.Database;

public class DatabaseContext : DbContext
{
    private readonly DatabaseOptions options;
    
    public DatabaseContext(
        DbContextOptions<DatabaseContext> options,
        IOptions<DatabaseOptions> dbOptionsAccessor
        ) : base(options)
    {
        this.options = dbOptionsAccessor.Value;
        Database.EnsureCreated();
    }

    public DbSet<NodeStorageElement> NodesStorage { get; set; }
    public DbSet<RootStorageElement> RootsStorage { get; set; }
    public DbSet<UserStorageElement> UsersStorage { get; set; }
}