using SMS.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMS.Dtos.Teachers
{
    public class TeacherDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public DateTime? BirthDate { get; set; }
       
        public long? SubjectNameId { get; set; }
       public string? SubjectName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
     
        public long? PositionId { get; set; }
        public string? PositionName { get; set; } 
        public long? salary { get; set; }

        public long? ManagerId { get; set; }
        public string? ManagerName { get; set; }
        public long? SuperVisedSectionId { get; set; }
        public string? SuperVisedSectionName { get; set; }
    }

    }
