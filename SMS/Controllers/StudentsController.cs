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
        //public List<Student> students = new List<Student>()
        //{
        //    new Student() { Id = 1, FirstName = "John", LastName = "Doe", BirthDate = new DateTime(1990, 01, 1) },
        //    new Student() { Id = 2, FirstName = "Jane", LastName = "Smith", BirthDate = new DateTime(1992, 5, 15) },
        //    new Student() { Id = 3, FirstName = "Michael", LastName = "Johnson", BirthDate = new DateTime(1988, 10, 20) }
        //};

        //Dependency Injection
        private readonly SchoolContext _dbcontext;
        public StudentsController(SchoolContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpGet("GetByCriteria")]
        public IActionResult GetByCriteria([FromQuery] SearchStudentDto studentDto )
        {
            try
            {
                var result = from student in _dbcontext.Student
                             from teacher in _dbcontext.Teacher.Where(t => t.Id == student.HomeroomTeacherId).DefaultIfEmpty()
                             from lookup in _dbcontext.Lookup.Where(l => l.Id == student.GradeId).DefaultIfEmpty()
                             from Section in _dbcontext.Lookup.Where(s => s.Id == student.SectionId).DefaultIfEmpty()
                             from Status in _dbcontext.Lookup.Where(s => s.Id == student.StatusId).DefaultIfEmpty()
                             where (studentDto.Name == null || student.FirstName == studentDto.Name)
                             orderby student.Id descending
                             select new StudentDto
                             {
                                 Id = student.Id,
                                 Name = student.FirstName + " " + student.LastName,
                                 Age = student.BirthDate.HasValue ? DateTime.Now.Year - student.BirthDate.Value.Year : null,
                                 BirthDate = student.BirthDate,
                                 GradeID = student.GradeId,
                                 GradeName = lookup.Name,
                                 SectionId = student.SectionId,
                                 SectionName = Section.Name,
                                 Email = student.Email,
                                 Phone = student.Phone,
                                 StatusId = student.StatusId,
                                 StatusName = Status.Name,
                                 HomeroomTeacherId = student.HomeroomTeacherId,
                                 TeacherName = teacher.FirstName + " " + teacher.LastName
                             };



                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        

        }
        [HttpGet("GetById/{id}")]
        public IActionResult GetById(long id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest("Invalid student ID");
                }

                var result = _dbcontext.Student.Select(x => new StudentDto
                {
                    Id = x.Id,
                    Name = x.FirstName + " " + x.LastName,
                    Age = x.BirthDate.HasValue ? DateTime.Now.Year - x.BirthDate.Value.Year : null,
                    BirthDate = x.BirthDate,
                    GradeID = x.GradeId,
                    GradeName = x.Lookups.Name,
                    SectionId = x.SectionId,
                    SectionName = x.Lookup.Name,
                    Email = x.Email,
                    Phone = x.Phone,
                    StatusId = x.StatusId,
                    StatusName = x.Status.Name,
                    HomeroomTeacherId = x.HomeroomTeacherId,
                    TeacherName = x.Teachers.FirstName + " " + x.Teachers.LastName


                }).FirstOrDefault(x => x.Id == id);

                if (result == null)
                {
                    return NotFound("Student not found");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

           
        }

        [HttpPost("Add")]
        public IActionResult Add([FromBody] SaveStudentDto studentDto)
        {
            try
            {
                var newStudent = new Student()
                {
                    Id = 0, //(students.LastOrDefault()?.Id ?? 0) ,
                    FirstName = studentDto.FirstName,
                    LastName = studentDto.LastName,
                    BirthDate = studentDto.BirthDate,
                    GradeId = studentDto.GradeId,
                    SectionId = studentDto.SectionId,
                    Email = studentDto.Email,
                    Phone = studentDto.Phone,
                    StatusId = studentDto.StatusId,
                    HomeroomTeacherId = studentDto.HomeroomTeacherId

                };
                _dbcontext.Student.Add(newStudent);
                _dbcontext.SaveChanges();
                return Ok();
            }
           
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("Update")]
        public IActionResult Update([FromBody] SaveStudentDto studentDto)
        {
            try {
                var student = _dbcontext.Student.FirstOrDefault(x => x.Id == studentDto.Id);

                if (student == null)
                {
                    return NotFound("Student not found");
                }

                student.FirstName = studentDto.FirstName;
                student.LastName = studentDto.LastName;
                student.BirthDate = studentDto.BirthDate;
                student.GradeId = studentDto.GradeId;
                student.SectionId = studentDto.SectionId;
                student.Email = studentDto.Email;
                student.Phone = studentDto.Phone;
                student.StatusId = studentDto.StatusId;
                student.HomeroomTeacherId = studentDto.HomeroomTeacherId;

                _dbcontext.SaveChanges();
                return Ok();
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("Delete")]
        public IActionResult Delete([FromQuery] long id)
        {
            try 
            {
                var student = _dbcontext.Student.FirstOrDefault(x => x.Id == id);

                if (student == null)
                {
                    return NotFound("Student not found");
                }

                _dbcontext.Student.Remove(student);
                _dbcontext.SaveChanges();
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }

    }
}
