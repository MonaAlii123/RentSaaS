using System.ComponentModel.DataAnnotations;

namespace employee.Models
{
    public class Employees
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Position { get; set; }
    }
}
