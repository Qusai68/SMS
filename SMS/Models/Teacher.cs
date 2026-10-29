using System.ComponentModel.DataAnnotations.Schema;

namespace SMS.Models
{
    public class Teacher
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        [ForeignKey("SubjectName")]
        public long SubjectNameId { get; set; }
        public Lookup SubjectName { get; set; } // navigation property
        public string? Email { get; set; }
        public string Phone { get; set; }
        [ForeignKey("Position")]
        public long PositionId { get; set; }
        public Lookup Position { get; set; } // navigation property
        public long salary { get; set; }
        [ForeignKey("Manager")]
        public long? ManagerId { get; set; }
        public Teacher Manager { get; set; } // navigation property
        [ForeignKey("SuperVisedSection")]
        public long SuperVisedSectionId { get; set; }
        public Lookup SuperVisedSection { get; set; } // navigation property
        [ForeignKey("SuperVisedGrade")]
        public long SuperVisedGradeId { get; set; }
        public Lookup SuperVisedGrade { get; set; } // navigation property

        [ForeignKey("User")]
        public long? UserId { get; set; } // Foreign key for User
        public User User { get; set; } // Navigation property for User




    }
}
