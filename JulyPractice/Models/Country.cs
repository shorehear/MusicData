using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JulyPractice
{
    public class Country
    {
        [Key]
        public Guid CountryID { get; set; }
        public string CountryName { get; set; }
        public ICollection<Musician> Musicians { get; set; }
    }
}
