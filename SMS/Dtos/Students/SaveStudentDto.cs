namespace SMS.Dtos.Students
{
    public class SaveStudentDto
    {
         public long? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long GradeId { get; set; }
        public long SectionId { get; set; }
        public string? Email { get; set; }
        public string Phone { get; set; }
        public long StatusId { get; set; }

        public long HomeroomTeacherId { get; set; }

        public DateTime? BirthDate { get; set; }
    }
}
