using JulyPractice;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace TestJulyPractice
{
    public class UnitTest : IDisposable
    {
        private readonly DbContextOptions<CurrentDbContext> options;
        private readonly CurrentDbContext context;

        public UnitTest()
        {
            options = new DbContextOptionsBuilder<CurrentDbContext>()
                           .UseSqlite("DataSource=:memory:")
                           .Options;

            context = new CurrentDbContext(options);
            context.Database.OpenConnection();
            FillDatabase(context);
        }

        private void FillDatabase(CurrentDbContext context)
        {
            PreparationToFill(context);

            var country = new Country { CountryID = 11, CountryName = "TestCountry" };
            context.Countries.Add(country);
            context.SaveChanges();

            var musician = new Musician { CountryID = 11, MusicianID = Guid.NewGuid(), Name = "TestMusician" };
            context.Musicians.Add(musician);
            context.SaveChanges();

            var album = new Album { AlbumID = Guid.NewGuid(), MusicianID = musician.MusicianID, Title = "TestAlbum" };
            context.Albums.Add(album);
            context.SaveChanges();

            var song = new Song { SongID = Guid.NewGuid(), MusicianID = musician.MusicianID, AlbumID = album.AlbumID, Title = "TestSong" };
            context.Songs.Add(song);
            context.SaveChanges();
        }

        private void PreparationToFill(CurrentDbContext context) 
        {
            context.Countries.RemoveRange(context.Countries);
            context.Musicians.RemoveRange(context.Musicians);
            context.Songs.RemoveRange(context.Songs);
            context.Albums.RemoveRange(context.Albums);
            context.SaveChanges();
        }

        [Fact]
        public void TestMusicianCount()
        {
            var musiciansCount = context.Musicians.Select(x => x.MusicianID).Count(); 
            Assert.Equal(1, musiciansCount);
        }

        [Fact]
        public void TestMusicianCountry()
        {
            var musician = context.Musicians.Include(m => m.Country).FirstOrDefault();
            Assert.NotNull(musician);
            Assert.Equal("TestCountry", musician.Country.CountryName);
        }

        [Fact]
        public void TestAddMultipleMusicians()
        {
            var country = context.Countries.First();
            var musician1 = new Musician { CountryID = country.CountryID, MusicianID = Guid.NewGuid(), Name = "Musician1" };
            var musician2 = new Musician { CountryID = country.CountryID, MusicianID = Guid.NewGuid(), Name = "Musician2" };

            context.Musicians.AddRange(musician1, musician2);
            context.SaveChanges();

            var musiciansCount = context.Musicians.Count();
            Assert.Equal(3, musiciansCount); 
        }

        [Fact]
        public void TestMusicianSongsAndAlbums()
        {
            var musician = context.Musicians.Include(m => m.Songs).Include(m => m.Albums).FirstOrDefault();
            Assert.NotNull(musician);
            Assert.Single(musician.Songs);
            Assert.Single(musician.Albums);
        }

        [Fact]
        public void TestDeleteMusician()
        {
            var musician = context.Musicians.FirstOrDefault();
            Assert.NotNull(musician);

            context.Musicians.Remove(musician);
            context.SaveChanges();

            var musiciansCount = context.Musicians.Count();
            Assert.Equal(0, musiciansCount);
        }
    }
}
