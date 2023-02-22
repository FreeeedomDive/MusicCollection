using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MusicCollection.UserService.Repositories.Database;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options, IOptions<DatabaseOptions> dbOptionsAccessor)
    {
        
    }
}