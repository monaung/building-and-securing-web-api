using Microsoft.EntityFrameworkCore;

namespace Lil.TimeTracking.Models
{
    public class TimeTrackingDbContext: DbContext
    {
        /*
            to navigate from this context
            and use context.xx to quickly access
            the whole set of xxx from the database
        */
       
       public TimeTrackingDbContext()
       {
        
       }

       public TimeTrackingDbContext(DbContextOptions options): base(options)
       {
         /*
            to initilized so that can create migration
            or run the script or create  the database 
         */
       }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TimeEntry> TimeEntries { get; set; }
    }
}