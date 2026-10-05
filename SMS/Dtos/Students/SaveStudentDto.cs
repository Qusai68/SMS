namespace SMS.Dtos.Students
{
    public class SaveStudentDto
    {
         public long? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime? BirthDate { get; set; }
    }
}
