using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JulyPractice
{
    public class Country
    {
        [Key]
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public ICollection<Musician> Musicians { get; set; }
    }

    public class Musician
    {
        [Key]
        public Guid MusicianID { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public int CountryID { get; set; }
        public DateTime BirthDate { get; set; }

        [ForeignKey("CountryID")]
        public Country Country { get; set; }
        public ICollection<Song> Songs { get; set; }
        public ICollection<Album> Albums { get; set; }
    }

    public class Song
    {
        [Key]
        public Guid SongID { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public Guid MusicianID { get; set; }
        public Guid AlbumID { get; set; }
        public int ReleaseYear { get; set; }

        [ForeignKey("MusicianID")]
        public Musician Musician { get; set; }

        [ForeignKey("AlbumID")]
        public Album Album { get; set; }
    }

    public class Album
    {
        [Key]
        public Guid AlbumID { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public Guid MusicianID { get; set; }
        public int ReleaseYear { get; set; }

        [ForeignKey("MusicianID")]
        public Musician Musician { get; set; }
        public ICollection<Song> Songs { get; set; }
    }
}
