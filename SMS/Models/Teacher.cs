namespace SMS.Models
{
    public class Teacher
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string SubjectName { get; set; }
        public string? Email { get; set; }
        public long Phone { get; set; }
        public string Position { get; set; }
        public long salary { get; set; }
        public long ManagerId { get; set; }

    }
}
