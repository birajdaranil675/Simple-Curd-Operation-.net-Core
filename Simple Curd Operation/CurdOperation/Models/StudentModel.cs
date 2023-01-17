namespace CurdOperation.Models
{
    public class StudentModel
    {
        public List<Student> StudentsList { get; set; }
    }

    public class Student 
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        public string? Subject { get; set; }

        public string? Standard { get; set; }
    }
}
