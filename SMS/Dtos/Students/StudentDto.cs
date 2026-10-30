using System.Diagnostics.Eventing.Reader;

namespace SMS.Dtos.Students
{
    public class StudentDto
    {
        public long? Id { get; set; }
        public string? Name { get; set; }
        public int? Age { get; set; }
        public DateTime? BirthDate { get; set; }
        public long? GradeID { get; set; }

        public long? SectionId { get; set; }
        public string? SectionName { get; set; }

        public string? Email { get; set; }
        public string? Phone { get; set; }
        public long? StatusId { get; set; }
        public string? StatusName { get; set; }
        public long? HomeroomTeacherId { get; set; }
        public string? TeacherName { get; set; }
         public string? GradeName { get; set; }

        public long? UserId { get; set; }

        public long? PositionId { get; set; }
        public string? PositionName { get; set; }



    }
}
