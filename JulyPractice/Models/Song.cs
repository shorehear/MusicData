using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JulyPractice
{
    public class Song
    {
        [Key]
        public Guid SongID { get; set; }
        public string Title { get; set; }
        public Guid MusicianID { get; set; }
        public Guid AlbumID { get; set; }

        [ForeignKey("MusicianID")]
        public Musician Musician { get; set; }

        [ForeignKey("AlbumID")]
        public Album Album { get; set; }
    }
}
