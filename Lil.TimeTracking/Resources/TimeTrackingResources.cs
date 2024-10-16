
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
    
    /*
        Resources to be used in Rest API

        These are represented when return or requests come in
    */
}
