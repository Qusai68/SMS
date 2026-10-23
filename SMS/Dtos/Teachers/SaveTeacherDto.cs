using SMS.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMS.Dtos.Teachers
{
    public class SaveTeacherDto
    {
        public long? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
       
        public long SubjectNameId { get; set; }
        
        public string? Email { get; set; }
        public string Phone { get; set; }
       
        public long PositionId { get; set; }
        
        public long salary { get; set; }

        public long? ManagerId { get; set; }
        public long SuperVisedSectionId { get; set; }

    }
}
