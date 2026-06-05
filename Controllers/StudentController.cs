using Microsoft.AspNetCore.Mvc;
using MyApi.Models;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/student")]
    public class StudentController : ControllerBase
    {
        private static List<Student> students = new()
        {
            new Student { Id = 1, Name = "An", Age = 20 },
            new Student { Id = 2, Name = "Binh", Age = 21 }
        };

        // GET: /api/student
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(students);
        }

        // GET: /api/student/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var student = students.FirstOrDefault(x => x.Id == id);
            if (student == null) return NotFound("Student not found");
            return Ok(student);
        }

        // POST: /api/student
        [HttpPost]
        public IActionResult Create(Student student)
        {
            student.Id = students.Max(x => x.Id) + 1;
            students.Add(student);
            return Ok(student);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Student student)
        {
            var existingStudent = students.FirstOrDefault(x => x.Id == id);
            if (existingStudent == null) return NotFound("Student not found");

            existingStudent.Name = student.Name;
            existingStudent.Age = student.Age;

            return Ok(existingStudent);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var student = students.FirstOrDefault(x => x.Id == id);
            if (student == null) return NotFound("Student not found");

            students.Remove(student);
            return Ok(new { message = "Xóa sinh viên thành công!" });
        }
    }
}