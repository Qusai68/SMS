using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMS.Dtos.Students;
using SMS.Dtos.Teachers;
using SMS.Models;

namespace SMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        public List<Teacher> Teachers = new List<Teacher>()
        {
            new Teacher() { Id = 1, FirstName = "John", LastName = "Doe", BirthDate = new DateTime(1990, 01, 1) },
            new Teacher() { Id = 2, FirstName = "Jane", LastName = "Smith", BirthDate = new DateTime(1992, 5, 15) },
            new Teacher() { Id = 3, FirstName = "Michael", LastName = "Johnson", BirthDate = new DateTime(1988, 10, 20) }
        };
        [HttpGet("GetByCriteria")]
        public IActionResult GetByCriteria([FromQuery] SearchTeacherDto searchDto)

        {
            var result = from teacher in Teachers
                         where (searchDto.Id == null || teacher.Id == searchDto.Id) &&
                               (searchDto.Name == null || teacher.FirstName == searchDto.Name)
                         orderby teacher.Id descending
                         select new TeacherDto
                         {
                             Id = teacher.Id,
                             Name = teacher.FirstName + " " + teacher.LastName,
                             BirthDate = teacher.BirthDate
                         };


            return Ok(result);
        }

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(long id)
        { 
            var teacher = Teachers.Select(x => new TeacherDto
            {
                Id = x.Id,
                Name = x.FirstName + " " + x.LastName,
                BirthDate = x.BirthDate
            }).FirstOrDefault(x => x.Id == id);

            if (teacher == null)
            {
                return NotFound("Teacher Does Not Exist");// 404
            }

            return Ok(teacher);
        
        }
        [HttpPost ("Add")]
        public IActionResult Add([FromBody]SaveTeacherDto saveDto)

        {
            var newTeacher = new Teacher
            {
                Id = (Teachers.LastOrDefault()?.Id ?? 0) + 1,
                FirstName = saveDto.FirstName,
                LastName = saveDto.LastName,
                BirthDate = saveDto.BirthDate
            };
            Teachers.Add(newTeacher);
            return Ok();
        }
        [HttpPut("Update")]
        public IActionResult Update( [FromBody] SaveTeacherDto saveDto)
        {
            var existingTeacher = Teachers.FirstOrDefault(x =>x.Id == saveDto.Id);
            if (existingTeacher == null)
            {
                return NotFound("Teacher Does Not Exist");
            }

            existingTeacher.FirstName = saveDto.FirstName;
            existingTeacher.LastName = saveDto.LastName;
            existingTeacher.BirthDate = saveDto.BirthDate;

            return Ok();
        }
        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(long id)
        {
            var teacher = Teachers.FirstOrDefault(x => x.Id == id);
            if (teacher == null)
            {
                return NotFound("Teacher Does Not Exist");
            }

            Teachers.Remove(teacher);
            return Ok();
        }

    }
}
