using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Windows;
using System.Linq;
//123456Aa

namespace JulyPractice
{
    public class CurrentDbContext : DbContext
    {
        public CurrentDbContext(DbContextOptions<CurrentDbContext> options) : base(options) { }
        public CurrentDbContext() { }
        public DbSet<User> Users { get; set; }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Musician> Musicians { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Album> Albums { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory; 
            string databasePath = Path.Combine(basePath, "JulyPractice.db");
            Logger.LogInformation($"Data source: {databasePath}");

            SQLitePCL.Batteries.Init();
            optionsBuilder.UseSqlite($"Data Source={databasePath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.ID);

            modelBuilder.Entity<Country>()
                .HasMany(c => c.Musicians)
                .WithOne(m => m.Country)
                .HasForeignKey(m => m.CountryID);

            modelBuilder.Entity<Musician>()
                .HasMany(m => m.Songs)
                .WithOne(s => s.Musician)
                .HasForeignKey(s => s.MusicianID);

            modelBuilder.Entity<Musician>()
                .HasMany(m => m.Albums)
                .WithOne(a => a.Musician)
                .HasForeignKey(a => a.MusicianID);

            modelBuilder.Entity<Album>()
                .HasMany(a => a.Songs)
                .WithOne(s => s.Album)
                .HasForeignKey(s => s.AlbumID);

            base.OnModelCreating(modelBuilder);
        }
    }
}
