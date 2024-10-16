using System.Collections.ObjectModel;

namespace Lil.TimeTracking.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateOnly StartDate { get; set; }
        public virtual Collection<Project> Projects { get; set; }
    }
}