using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMS.DbContexts;
using SMS.Dtos.Students;
using SMS.Models;
using System.Security.Claims;

namespace SMS.Controllers
{
    [Authorize]
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
                var role = User.FindFirst(ClaimTypes.Role)?.Value;
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;



                var result = from student in _dbcontext.Student
                             from teacher in _dbcontext.Teacher.Where(t => t.Id == student.HomeroomTeacherId).DefaultIfEmpty()
                             from lookup in _dbcontext.Lookup.Where(l => l.Id == student.GradeId).DefaultIfEmpty()
                             from Section in _dbcontext.Lookup.Where(s => s.Id == student.SectionId).DefaultIfEmpty()
                             from Status in _dbcontext.Lookup.Where(s => s.Id == student.StatusId).DefaultIfEmpty()
                             where 
                             (studentDto.Name == null || student.FirstName .Contains (studentDto.Name)) &&
                             (studentDto.Grade == null || lookup.Name == studentDto.Grade.ToString() &&
                             ( studentDto.Section == null || Section.Name == studentDto.Section.ToString())
                             
                             )
                             orderby student.Id descending
                             select new StudentDto
                             {
                                 Id = student.Id,
                                 Name = student.FirstName + " " + student.LastName,
                                 Age = (int)((DateTime.Now - student.BirthDate).TotalDays / 365.25),
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
                                 TeacherName = student.FirstName + " " + student.LastName,
                                 UserId = student.UserId,
                                 PositionId = student.PositionId
                             };



                if (role?.ToUpper() != "HR" && role?.ToUpper() != "ADMIN")
                {
                    result = result.Where(t => t.UserId == long.Parse(userId));
                }


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

                var role = User.FindFirst(ClaimTypes.Role)?.Value;
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


                if (id == 0)
                {
                    return BadRequest("Invalid student ID");
                }

                var result = _dbcontext.Student.Select(x => new StudentDto
                {
                    Id = x.Id,
                    Name = x.FirstName + " " + x.LastName,
                    Age = (int)((DateTime.Now - x.BirthDate).TotalDays / 365.25),
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
                    TeacherName = x.Teachers.FirstName + " " + x.Teachers.LastName,
                    UserId = x.UserId,
                    PositionId = x.Position.Id,
                    PositionName = x.Position.Name
                }).FirstOrDefault(x => x.Id == id);

                if (result == null)
                {
                    return NotFound("Student not found");
                }

                if (role?.ToUpper() != "HR" && role?.ToUpper() != "ADMIN")
                {
                    if (result.UserId != long.Parse(userId))
                    {
                        return Forbid();// You cant access this teacher because you are not HR or Admin and you are not the owner of this teacher record
                    }
                }


                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

           
        }
        [Authorize(Roles = "HR,Admin")]
        [HttpPost("Add")]
        public IActionResult Add([FromBody] SaveStudentDto studentDto)
        {
            try
            {

                var user = new User()
                {
                    Id = 0,
                    Username = $"{studentDto.FirstName}_{studentDto.LastName}_SMS",
                    HashedPassword = BCrypt.Net.BCrypt.HashPassword($"{studentDto.FirstName}@SMS"),
                    IsAdmin = false
                };

                var isUserExists = _dbcontext.Users.Any(u => u.Username.ToUpper() == user.Username.ToUpper());
                if (isUserExists)
                {
                    return BadRequest("User already exists");
                }
                _dbcontext.Users.Add(user);

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
                    HomeroomTeacherId = studentDto.HomeroomTeacherId,
                    User = user,
                    PositionId = studentDto.PositionId

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

        [Authorize(Roles = "HR,Admin")]
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
                student.PositionId = studentDto.PositionId;

                _dbcontext.SaveChanges();
                return Ok();
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Authorize(Roles = "HR,Admin")]
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
                if (student.User != null)
                {
                    _dbcontext.Users.Remove(student.User);
                }
                var studentUserAssociate = _dbcontext.Teacher.Include(t => t.User).FirstOrDefault(x => x.Id == student.UserId);
                if (student.User != null)
                {
                    _dbcontext.Users.Remove(student.User);
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
        //// This method hashes a password using BCrypt and returns the hashed password.
        //[HttpGet("HashedPassword")]
        //public IActionResult HashedPassword(string password)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(password))
        //        {
        //            return BadRequest("Password cannot be null or empty");
        //        }
        //        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        //        return Ok(hashedPassword);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

    }
}
