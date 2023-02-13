using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MusicCollection.Repositories.Database;

namespace MusicCollection.UsersService.Repositories.Database;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options, IOptions<DatabaseOptions> dbOptionsAccessor)
    {
        
    }
}