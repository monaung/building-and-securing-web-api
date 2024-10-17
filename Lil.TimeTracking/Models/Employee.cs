using System.Collections.ObjectModel;

namespace Lil.TimeTracking.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateOnly StartDate { get; set; }
        public virtual ICollection<Project> Projects { get; set; }

        /*
            Virtual members define related entities 
            which Entity Framework can use to create database table keys and relationships.

            Using virtual members in your database entities allows 
            Entity Framework (EF) to take advantage of lazy loading. 
            Lazy loading means that the related data is only fetched from the database 
            when you actually access that property for the first time. 
            Without virtual members, EF would have to eagerly load all related data, 
            which could lead to unnecessary database queries and performance hits.
        */
    }
}