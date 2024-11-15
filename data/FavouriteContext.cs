using Microsoft.EntityFrameworkCore;

namespace WebApi.Models;

public class FavouriteContext : DbContext
{
    public FavouriteContext(DbContextOptions<FavouriteContext> options)
        : base(options)
    {
    }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // Connexion a la base sqlite
        options.UseSqlite("Data Source=Favourite.db");
    }
    public DbSet<Favourite> Favourites { get; set; } = null!;
}