namespace SMS.Dtos.Teachers
{
    public class SaveTeacherDto
    {
        public long? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
