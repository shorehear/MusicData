using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JulyPractice
{
    public class Album
    {
        [Key]
        public Guid AlbumID { get; set; }
        public string Title { get; set; }
        public Guid MusicianID { get; set; }
        public int ReleaseYear { get; set; }

        [ForeignKey("MusicianID")]
        public Musician Musician { get; set; }
        public ICollection<Song> Songs { get; set; }
    }
}
