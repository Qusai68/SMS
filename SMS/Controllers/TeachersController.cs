using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SMS.DbContexts;
using SMS.Dtos.Students;
using SMS.Dtos.Teachers;
using SMS.Models;
using System.Security.Claims;

namespace SMS.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        //public List<Teacher> Teachers = new List<Teacher>()
        //{
        //    new Teacher() { Id = 1, FirstName = "John", LastName = "Doe", BirthDate = new DateTime(1990, 01, 1) },
        //    new Teacher() { Id = 2, FirstName = "Jane", LastName = "Smith", BirthDate = new DateTime(1992, 5, 15) },
        //    new Teacher() { Id = 3, FirstName = "Michael", LastName = "Johnson", BirthDate = new DateTime(1988, 10, 20) }
        //};
        private readonly SchoolContext _dbcontext;
        public TeachersController(SchoolContext dbcontext)
        {
            _dbcontext = dbcontext;
        }


        
        [HttpGet("GetByCriteria")]
        public IActionResult GetByCriteria([FromQuery] SearchTeacherDto searchDto)

        {
            try
            {
                var role = User.FindFirst(ClaimTypes.Role)?.Value;
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;



                var result = from teacher in _dbcontext.Teacher
                             from lookup in _dbcontext.Lookup.Where(l => l.Id == teacher.PositionId).DefaultIfEmpty()
                             from subject in _dbcontext.Lookup.Where(s => s.Id == teacher.SubjectNameId).DefaultIfEmpty()
                             from Manager in _dbcontext.Teacher.Where(m => m.Id == teacher.ManagerId).DefaultIfEmpty()
                             from SuperVisedSection in _dbcontext.Lookup.Where(s => s.Id == teacher.SuperVisedSectionId).DefaultIfEmpty()
                             from SuperVisedGrade in _dbcontext.Lookup.Where(s => s.Id == teacher.SuperVisedGradeId).DefaultIfEmpty()
                             where (searchDto.Id == null || teacher.Id == searchDto.Id) &&
                                   (searchDto.Position == null || lookup.Name == searchDto.Position)
                             orderby teacher.Id descending
                             select new TeacherDto
                             {
                                 Id = teacher.Id,
                                 Name = teacher.FirstName + " " + teacher.LastName,
                                 BirthDate = teacher.BirthDate,
                                 SubjectNameId = teacher.SubjectNameId,
                                 SubjectName = subject.Name, // Use the name from the subject lookup table
                                 Email = teacher.Email,
                                 Phone = teacher.Phone,
                                 PositionId = teacher.PositionId,
                                 PositionName = lookup.Name, // Use the name from the lookup table
                                 salary = teacher.salary,
                                 ManagerId = teacher.ManagerId,
                                 ManagerName = Manager.FirstName + " " + Manager.LastName, // Use the name from the manager teacher table
                                 SuperVisedSectionId = teacher.SuperVisedSectionId,
                                 SuperVisedSectionName = SuperVisedSection.Name,// Use the name from the supervised section teacher table
                                 SuperVisedGradeId = teacher.SuperVisedGradeId,
                                 SuperVisedGradeName = SuperVisedGrade.Name, // Use the name from the supervised grade teacher table
                                 UserId = teacher.UserId // Include the UserId property

                             };
                if (role?.ToUpper() != "HR" && role?.ToUpper() != "ADMIN")
                {
                   result = result.Where(t => t.UserId == long.Parse(userId));
                }

                return Ok(result);
            }
            catch(Exception ex)
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


                var teacher = _dbcontext.Teacher.Select(x => new TeacherDto
                {
                    Id = x.Id,
                    Name = x.FirstName + " " + x.LastName,
                    BirthDate = x.BirthDate,
                    SubjectNameId = x.SubjectNameId,
                    SubjectName = x.SubjectName.Name,
                    Email = x.Email,
                    Phone = x.Phone,
                    PositionId = x.PositionId,
                    PositionName = x.Position.Name,
                    salary = x.salary,
                    ManagerId = x.ManagerId,
                    ManagerName = x.Manager.FirstName + " " + x.Manager.LastName,
                    SuperVisedSectionId = x.SuperVisedSectionId,
                    SuperVisedSectionName = x.SuperVisedSection.Name,
                    SuperVisedGradeId = x.SuperVisedGradeId,
                    SuperVisedGradeName = x.SuperVisedGrade.Name

                }).FirstOrDefault(x => x.Id == id);

                if (teacher == null)
                {
                    return NotFound("Teacher Does Not Exist");// 404
                }

                if (role?.ToUpper() != "HR" && role?.ToUpper() != "ADMIN")
                {
                    if(teacher.UserId != long.Parse(userId))
                    {
                        return Forbid();// You cant access this teacher because you are not HR or Admin and you are not the owner of this teacher record
                    }
                }



                return Ok(teacher);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        
        }
        [Authorize(Roles = "HR,Admin")]
        [HttpPost ("Add")]
        public IActionResult Add([FromBody]SaveTeacherDto saveDto)

        {
            try
            {
                var user = new User()
                {
                    Id = 0,
                    Username = $"{saveDto.FirstName}_{saveDto.LastName}_SMS",
                    HashedPassword = BCrypt.Net.BCrypt.HashPassword($"{saveDto.FirstName}@SMS"),
                    IsAdmin = false

                };
                var isUserExists = _dbcontext.Users.Any(u => u.Username.ToUpper() == user.Username.ToUpper());
                if (isUserExists)
                {
                    return BadRequest("User already exists");
                }
                _dbcontext.Users.Add(user);


                var newTeacher = new Teacher
                {
                    Id = 0,
                    FirstName = saveDto.FirstName,
                    LastName = saveDto.LastName,
                    BirthDate = saveDto.BirthDate,
                    SubjectNameId = saveDto.SubjectNameId,
                    Email = saveDto.Email,
                    Phone = saveDto.Phone,
                    PositionId = saveDto.PositionId,
                    salary = saveDto.salary,
                    ManagerId = saveDto.ManagerId,
                    SuperVisedSectionId = saveDto.SuperVisedSectionId,
                    SuperVisedGradeId = saveDto.SuperVisedGradeId,
                    // UserId = user.Id
                    User = user
                };
                _dbcontext.Teacher.Add(newTeacher);
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
        public IActionResult Update( [FromBody] SaveTeacherDto saveDto)
        {
            try
            {
                var existingTeacher = _dbcontext.Teacher.FirstOrDefault(x => x.Id == saveDto.Id);
                if (existingTeacher == null)
                {
                    return NotFound("Teacher Does Not Exist");
                }

                existingTeacher.FirstName = saveDto.FirstName;
                existingTeacher.LastName = saveDto.LastName;
                existingTeacher.BirthDate = saveDto.BirthDate;
                existingTeacher.SubjectNameId = saveDto.SubjectNameId;
                existingTeacher.Email = saveDto.Email;
                existingTeacher.Phone = saveDto.Phone;
                existingTeacher.PositionId = saveDto.PositionId;
                existingTeacher.salary = saveDto.salary;
                existingTeacher.ManagerId = saveDto.ManagerId;
                existingTeacher.SuperVisedSectionId = saveDto.SuperVisedSectionId;
                existingTeacher.SuperVisedGradeId = saveDto.SuperVisedGradeId;
                _dbcontext.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


            
        }
        [Authorize(Roles = "HR,Admin")]
        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(long id)
        {
            try
            {
                var teacher = _dbcontext.Teacher.FirstOrDefault(x => x.Id == id);
                if (teacher == null)
                {
                    return NotFound("Teacher Does Not Exist");
                }

                var teacherAssociate = _dbcontext.Teacher.Any(x => x.ManagerId == id);
                if(teacherAssociate)
                {
                    return BadRequest("Cannot delete teacher because it assigned Students");
                }

                _dbcontext.Teacher.Remove(teacher);
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
