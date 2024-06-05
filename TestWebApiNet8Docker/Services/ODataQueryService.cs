using Microsoft.AspNetCore.OData.Query;

namespace TestWebApiNet8Docker.Services
{
    public record Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int Score { get; set; }
    }

    public class ODataQueryService
    {
        private List<Student> students = new()
        {
            new Student
            {
                Id = 1,
                Name = "Alice",
                Score = 90
            },
            new Student
            {
                Id = 1,
                Name = "Bob",
                Score = 100
            },
            new Student
            {
                Id = 1,
                Name = "Per",
                Score = 150
            }
        };

        public List<Student> GetStudents(ODataQueryOptions<Student> options)
        {
            var query = students.AsQueryable();
            var result = options.ApplyTo(query) as IQueryable<Student>;

            if (result != null)
                return result.ToList();
            else
                return new List<Student>();
        }
    }
}
