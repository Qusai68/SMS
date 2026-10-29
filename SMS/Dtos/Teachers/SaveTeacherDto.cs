using SMS.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMS.Dtos.Teachers
{
    public class SaveTeacherDto
    {
        public long? Id { get; set; }

        [Required, MaxLength(100)]
        public string FirstName { get; set; }
        [Required, MaxLength(100)]
        public string LastName { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
       
        public long SubjectNameId { get; set; }
        
        public string? Email { get; set; }
        [Required, MaxLength(30)]
        public string Phone { get; set; }
       
        public long PositionId { get; set; }

        [Range(0, long.MaxValue)]
        public long salary { get; set; }

        public long? ManagerId { get; set; }
        public long SuperVisedSectionId { get; set; }
        public long SuperVisedGradeId { get; set; }

    }
}
