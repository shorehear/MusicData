using System;
using System.ComponentModel.DataAnnotations;

namespace JulyPractice
{
    public class User
    {
        [Key]
        public Guid ID { get; set; }
        
        public string Username { get; set; }
        public string PasswordHash { get; set; }

    }
}
