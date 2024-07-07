using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Windows;
using System.Linq;


namespace JulyPractice
{
    public class CurrentDbContext : DbContext
    {
        //пользователь
        public DbSet<User> Users { get; set; }

        //датасет
        public DbSet<Country> Countries { get; set; }
        public DbSet<Musician> Musicians { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Album> Albums { get; set; }

        public CurrentDbContext(DbContextOptions<CurrentDbContext> options) : base(options) 
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            SQLitePCL.Batteries.Init();
            optionsBuilder.UseSqlite("Data Source=JulyPractice.db");
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

            SeedFromFiles(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        #region Заполнить бд
        public void SeedFromFiles(ModelBuilder modelBuilder)
        {
            using (var transaction = Database.BeginTransaction())
            {
                try
                {
                    SeedCountries(Path.Combine(basePath, "Countries.txt"));
                    SeedMusicians(Path.Combine(basePath, "Musicians.txt"));
                    SeedAlbums(Path.Combine(basePath, "Albums.txt"));
                    SeedSongs(Path.Combine(basePath, "Songs.txt"));

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: не удалось заполнить БД. {ex.Message}");
                    transaction.Rollback();
                }
            }
        }

        private void SeedCountries(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            var countries = lines.Skip(1).Select(line => new Country
            {
                CountryName = line.Trim()
            }).ToList();

            foreach (var country in countries) 
            {
                if (!CountryExists(country.CountryName))
                {
                    Countries.Add(country);
                }
            }
        }

        private void SeedMusicians(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            var musicians = lines.Skip(1).Select(line =>
            {
                var parts = line.Split(',');
                return new Musician
                {
                    Name = parts[0].Trim(),
                    CountryID = int.Parse(parts[1].Trim()),
                    BirthDate = DateTime.Parse(parts[2].Trim())
                };
            }).ToList();

            foreach (var musician in musicians)
            {
                if (!MusicianExists(musician.Name, musician.CountryID))
                {
                    Musicians.Add(musician);
                }
            }
        }

        private void SeedAlbums(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            var albums = lines.Skip(1).Select(line =>
            {
                var parts = line.Split(',');
                var title = parts[0].Trim();
                var releaseYear = int.Parse(parts[1].Trim());

                var existingAlbum = Albums.FirstOrDefault(a => a.Title == title);
                if (existingAlbum != null)
                {
                    return existingAlbum; 
                }

                var newAlbum = new Album
                {
                    AlbumID = Guid.NewGuid(),
                    Title = title,
                    ReleaseYear = releaseYear
                };

                return newAlbum;
            }).ToList();

            foreach (var album in albums)
            {
                if (!AlbumExists(album.Title))
                {
                    Albums.Add(album);
                }
            }
        }

        private void SeedSongs(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            var songs = lines.Skip(1).Select(line =>
            {
                var parts = line.Split(',');
                var title = parts[0].Trim();
                var releaseYear = int.Parse(parts[1].Trim());

                var existingSong = Songs.FirstOrDefault(s => s.Title == title);
                if (existingSong != null)
                {
                    return existingSong; 
                }

                var newSong = new Song
                {
                    MusicianID = Guid.NewGuid(),
                    AlbumID = Guid.NewGuid(),
                    Title = title,
                    ReleaseYear = releaseYear
                };

                return newSong;
            }).ToList();

            foreach (var song in songs)
            {
                if (!SongExists(song.Title))
                {
                    Songs.Add(song);
                }
            }
        }
        #region Проверка существования данных
        private bool CountryExists(string countryName)
        {
            return Countries.Any(c => c.CountryName == countryName);
        }

        private bool MusicianExists(string MusicianName, int countryId)
        {
            return Musicians.Any(m=>m.Name == MusicianName && m.CountryID == countryId);
        }

        private bool SongExists(string title)
        {
            return Songs.Any(s => s.Title == title);
        }

        private bool AlbumExists(string title) 
        { 
            return Albums.Any(a => a.Title == title);
        }
        #endregion
        #endregion
    }
}
