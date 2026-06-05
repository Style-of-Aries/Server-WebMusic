using MyApi.Models;

namespace MyApi.Repositories
{
    public class StudentRepository
    {
        private readonly List<Student> _students = new List<Student>();

        public IEnumerable<Student> GetAllStudents()
        {
            return _students;
        }

        public Student? GetStudentById(int id)
        {
            return _students.FirstOrDefault(s => s.Id == id);
        }

        public void AddStudent(Student student)
        {
            _students.Add(student);
        }

        public void UpdateStudent(Student student)
        {
            var existingStudent = GetStudentById(student.Id);
            if (existingStudent != null)
            {
                _students.Remove(existingStudent);
                _students.Add(student);
            }
        }

        public void DeleteStudent(int id)
        {
            var student = GetStudentById(id);
            if (student != null)
            {
                _students.Remove(student);
            }
        }
    }
}