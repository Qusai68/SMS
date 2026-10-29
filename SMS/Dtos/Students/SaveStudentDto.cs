using System.ComponentModel.DataAnnotations;

namespace SMS.Dtos.Students
{
    public class SaveStudentDto
    {
         public long? Id { get; set; }

        [Required, MaxLength(100)]
        public string FirstName { get; set; }
        [Required, MaxLength(100)]
        public string LastName { get; set; }

        [Range(1, long.MaxValue)]
        public long GradeId { get; set; }

        [Range(1, long.MaxValue)]
        public long SectionId { get; set; }
        [EmailAddress,MaxLength(100)]
        public string? Email { get; set; }
        [Required, Phone]
        public string Phone { get; set; }

        [Range(1, long.MaxValue)]
        public long StatusId { get; set; }

        public long HomeroomTeacherId { get; set; }

        public DateTime BirthDate { get; set; }
    }
}
