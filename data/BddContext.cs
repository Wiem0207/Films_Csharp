using WebApi.Models ;
using Microsoft.EntityFrameworkCore; 
public class BddContext : DbContext
{
	public BddContext(DbContextOptions<BddContext> options)
		: base(options)
	{
	}

	protected override void OnConfiguring(DbContextOptionsBuilder options)
	{
		// Connexion a la base sqlite
		options.UseSqlite("Data Source=Bdd.db");
	}

	public DbSet<User> Users { get; set; } = null!;
    public DbSet<Film> Films { get; set; } = null!;
    public DbSet<Favourite> Favourites { get; set; } = null!;
}