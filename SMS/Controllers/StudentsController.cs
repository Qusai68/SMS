using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMS.Models;
using SMS.Dtos.Students;
using SMS.DbContexts;

namespace SMS.Controllers
{
    [Route("api/[controller]")] // Data Annotation
    [ApiController]// data annotation
    public class StudentsController : ControllerBase
    {
        public List<Student> students = new List<Student>()
        {
            new Student() { Id = 1, FirstName = "John", LastName = "Doe", BirthDate = new DateTime(1990, 01, 1) },
            new Student() { Id = 2, FirstName = "Jane", LastName = "Smith", BirthDate = new DateTime(1992, 5, 15) },
            new Student() { Id = 3, FirstName = "Michael", LastName = "Johnson", BirthDate = new DateTime(1988, 10, 20) }
        };

        //Dependency Injection
        private readonly SchoolContext _dbcontext;
        public StudentsController(SchoolContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpGet("GetByCriteria")]
        public IActionResult GetByCriteria([FromQuery] SearchStudentDto studentDto )
        {
            var result = from student in _dbcontext.Students
                         where (studentDto.Name == null || student.FirstName == studentDto.Name)
                         orderby student.Id descending
                         select new StudentDto
                         { 
                             Id = student.Id,
                             Name = student.FirstName + " " + student.LastName,
                             Age = student.BirthDate.HasValue ? DateTime.Now.Year - student.BirthDate.Value.Year : null,
                             BirthDate = student.BirthDate

                         };


            return Ok(result); 
        

        }
        [HttpGet("GetById/{id}")]
        public IActionResult GetById(long id)
        {
            if (id == 0)
            {
                return BadRequest("Invalid student ID");
            }

            var result = students.Select(x => new StudentDto
            {
                Id = x.Id,
                Name = x.FirstName + " " + x.LastName,
                Age = x.BirthDate.HasValue ? DateTime.Now.Year - x.BirthDate.Value.Year : null,
                BirthDate = x.BirthDate
            }).FirstOrDefault(x => x.Id == id);

            if(result == null)
            {
                return NotFound("Student not found");
            }

            return Ok(result);
        }

        [HttpPost("Add")]
        public IActionResult Add([FromBody] SaveStudentDto studentDto)
        {
            var newStudent = new Student()
            {
                Id = (students.LastOrDefault()?.Id ?? 0) + 1,
                FirstName = studentDto.FirstName,
                LastName = studentDto.LastName,
                BirthDate = studentDto.BirthDate
            };
            students.Add(newStudent);
            return Ok();

        }
        [HttpPut("Update")]
        public IActionResult Update([FromBody] SaveStudentDto studentDto)
        {
            var student = students.FirstOrDefault(x => x.Id == studentDto.Id);

            if (student == null)
            {
                return NotFound("Student not found");
            }

            student.FirstName = studentDto.FirstName;
            student.LastName = studentDto.LastName;
            student.BirthDate = studentDto.BirthDate;

            return Ok();
        }

        [HttpDelete("Delete")]
        public IActionResult Delete([FromQuery] long id)
        {
            var student = students.FirstOrDefault(x => x.Id == id);

            if (student == null)
            {
                return NotFound("Student not found");
            }

            students.Remove(student);
            return Ok();
        }

    }
}
