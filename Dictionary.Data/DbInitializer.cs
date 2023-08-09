using Dictionary.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Dictionary.Data;

public class DbInitializer
{
    private readonly ApplicationDbContext _db;

    public DbInitializer(ApplicationDbContext db)
    {
        _db = db;
    }
    
    public async Task InitializeAsync()
    {
        await _db.Database.MigrateAsync();
    }
}