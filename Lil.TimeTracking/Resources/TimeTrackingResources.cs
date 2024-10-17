
namespace Lil.TimeTracking.Resources
{
    /*
       record is not class or struct

        immutable, can't change once it set

       constructor type definition with Properties
    */

    public record Employee(int Id, string Name, DateOnly StartDate);

    public record Project(int Id, string Name, DateTime StartDate, DateTime? EndDate);

    public record TimeEntry(Guid Id, int EmployeeId, int ProjectId, DateOnly DateWorked, decimal HoursWorked);

    public record ProjectAssignment (int EmployeeId, int ProjectId, string? EmployeeName, string? ProjectName);
    
    public record Resource(string name, string url);

    public record LinkedResource<T>{

        public LinkedResource(T resource)
        {
            Data = resource;
            Links = new List<Resource>();
        }
        public T Data { get; set; }
        public List<Resource> Links { get; set; }
    }
    /*
        Resources to be used in Rest API

        These are represented when return or requests come in


        Why do we have to map between the resource classes and the database model classes?
        we don't want to expose our database model to clients, 
        and we often have resource representations that do not map directly to a single model class

        Exposing the database model provides tight coupling between the client and 
        an implementation detail whereas mapping to resource types keeps the client 
        interface independent of the database schema for the most part.
    */
}
