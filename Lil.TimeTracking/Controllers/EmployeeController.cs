using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elfie.Serialization;
using Lil.TimeTracking.Models;
using Mapster;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lil.TimeTracking.Controllers
{
    /*
        Controllers provide a centralized class to manage all the operations on a resource.
        Controllers provide a clean way to organize all the operations on a resource and apply routes and security.
    */

    [ApiController]
    [Route("api/[controller]")]

    public class EmployeeController : ControllerBase
    {
        private readonly TimeTrackingDbContext context;

        public EmployeeController(TimeTrackingDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [ProducesResponseType<IEnumerable<Resources.Employee>>(StatusCodes.Status200OK)]
        public async Task<ActionResult> Get()
        {
            var response = context.Employees.ProjectToType<Resources.Employee>().AsEnumerable();

            var linkedEmployees = new List<Resources.LinkedResource<Resources.Employee>>();
            foreach (var e in response)
            {
                var lEmp = new Resources.LinkedResource<Resources.Employee>(e);
                lEmp.Links.Add(new Resources.Resource("Projects", $"/api/Employee/{e.Id}/Projects"));
                linkedEmployees.Add(lEmp);
            }
            return Ok(linkedEmployees);
        }

        [HttpGet("{id}")]
        [ProducesResponseType<Resources.Employee>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var dbEmployee = await context.Employees.FindAsync(id);

            if (dbEmployee == null) return NotFound();

            var response = dbEmployee.Adapt<Resources.Employee>();

            var linkedEmployee = new Resources.LinkedResource<Resources.Employee>(response);
            linkedEmployee.Links.Add(new Resources.Resource("Projects", $"/api/Empmloyee/{response.Id}/Projects"));

            return Ok(linkedEmployee);
        }

        [HttpGet("{id}/Projects")]
        [ProducesResponseType<IEnumerable<Resources.Project>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProjects(int id)
        {
            var dbEmployee = await context.Employees.FindAsync(id);

            if (dbEmployee == null)
            {
                return NotFound();
            }
            else
            {
                await context.Entry(dbEmployee).Collection(e=> e.Projects).LoadAsync();
                var projects = new List<Resources.Project>();

                foreach (var p in dbEmployee.Projects)
                {
                    var rProject = p.Adapt<Resources.Project>();
                    projects.Add(rProject);
                }

                return Ok(projects);
            }
        }

        [HttpPost]
        [ProducesResponseType<Resources.Employee>(StatusCodes.Status201Created)]
        [ProducesResponseType<ObjectResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ObjectResult>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] Resources.Employee value)
        {
            if (!ModelState.IsValid)
            {
                return Problem("Invalid employee request", statusCode: StatusCodes.Status400BadRequest);
            }

            try
            {


                var dbEmployee = value.Adapt<Models.Employee>();

                await context.Employees.AddAsync(dbEmployee);
                await context.SaveChangesAsync();

                var response = dbEmployee.Adapt<Resources.Employee>();

                return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
            }
            catch (System.Exception)
            {

                return Problem("Problem persisting employee resource.", statusCode: StatusCodes.Status500InternalServerError);
            }
        }


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType<Resources.Employee>(StatusCodes.Status204NoContent)]
        [ProducesResponseType<ObjectResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ObjectResult>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, [FromBody] Resources.Employee value)
        {
            if (!ModelState.IsValid)
            {
                return Problem("Invalid employee request", statusCode: StatusCodes.Status400BadRequest);
            }

            try
            {
                var dbEmployee = value.Adapt<Models.Employee>();

                context.Entry<Models.Employee>(dbEmployee).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await context.SaveChangesAsync();

                return NoContent();

            }
            catch (DbUpdateConcurrencyException dbEx)
            {
                var dbEmployee = context.Employees.Find(id);
                if (dbEmployee == null)
                {
                    return NotFound();
                }
                else
                {
                    return Problem("Problem persisting employee resource.", statusCode: StatusCodes.Status500InternalServerError);

                }
            }
            catch (System.Exception)
            {

                return Problem("Problem persisting employee resource.", statusCode: StatusCodes.Status500InternalServerError);
            }
        }


        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType<Resources.Employee>(StatusCodes.Status204NoContent)]
        [ProducesResponseType<ObjectResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ObjectResult>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<Resources.Employee> value)
        {
            if (value == null)
            {
                return Problem("Invalid employee request", statusCode: StatusCodes.Status400BadRequest);
            }

            try
            {

                var dbEmployee = context.Employees.Find(id);
                if (dbEmployee == null)
                {
                    return NotFound();
                }

                var employee = dbEmployee.Adapt<Resources.Employee>();
                value.ApplyTo(employee, ModelState);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var patchedEmployee = employee.Adapt<Models.Employee>();

                context.Entry<Models.Employee>(dbEmployee).CurrentValues.SetValues(patchedEmployee);

                await context.SaveChangesAsync();

                return NoContent();

            }
            catch (System.Exception)
            {

                return Problem("Problem persisting employee resource.", statusCode: StatusCodes.Status500InternalServerError);
            }
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType<Resources.Employee>(StatusCodes.Status204NoContent)]
        [ProducesResponseType<ObjectResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ObjectResult>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var dbEmployee = await context.Employees.FindAsync(id);
                if (dbEmployee == null)
                    return NotFound();

                context.Employees.Remove(dbEmployee);
                await context.SaveChangesAsync();

                return NoContent();
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
    }
}