using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace SMS.Models
{
    public class Student
    {

        public long Id { get; set; }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        
        public string? Email { get; set; }
    
        public string Phone { get; set; }
        [ForeignKey("Status")] // navigation property]
        public long StatusId { get; set; }

        public Lookup Status { get; set; }

        [ForeignKey("Lookup")] // navigation property
        public long SectionId { get; set; }

        public Lookup Lookup { get; set; }

        [ForeignKey("Lookups")]
        public long GradeId { get; set; }

        public Lookup Lookups { get; set; }
        [ForeignKey("Teachers")]
        public long HomeroomTeacherId { get; set; }

        public Teacher Teachers { get; set; }
        public DateTime BirthDate { get; set; }

        [ForeignKey("User")]
        public long? UserId { get; set; } // Foreign key for User

        public User User { get; set; } // Navigation property for User

        [ForeignKey("Position")]
        public long PositionId { get; set; }
        public Lookup Position { get; set; } // navigation property
    }
}
