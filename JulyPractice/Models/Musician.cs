using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JulyPractice
{
    public class Musician
    {
        [Key]
        public Guid MusicianID { get; set; }
        public string Name { get; set; }
        public int CountryID { get; set; }

        [ForeignKey("CountryID")]
        public Country Country { get; set; }
        public ICollection<Song> Songs { get; set; }
        public ICollection<Album> Albums { get; set; }
    }
}
