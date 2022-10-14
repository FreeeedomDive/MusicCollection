using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MusicCollection.BusinessLogic.Repositories.Auth;
using MusicCollection.BusinessLogic.Repositories.Files.Nodes;
using MusicCollection.BusinessLogic.Repositories.Files.Roots;

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
        Database.EnsureCreated();
    }
        
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(Options.ConnectionString);
    }

    public DbSet<NodeStorageElement> NodesStorage { get; set; }
    public DbSet<RootStorageElement> RootsStorage { get; set; }
    public DbSet<UserStorageElement> UsersStorage { get; set; }
}