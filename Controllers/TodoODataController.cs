using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoOData.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNet.OData.Routing;

namespace TodoOData.Controllers
{
    [EnableCors]
    [ODataRoutePrefix("todos")]
    public class TodosController : ODataController
    {
        private readonly TodoODataContext _context;

        public TodosController(TodoODataContext context)
        {
            _context = context;
        }

        [EnableQuery]
        [ODataRoute]
        public IEnumerable<Todo> GetTodoList()
        {
            return _context.TodoList;
        }

        [EnableQuery]
        [ODataRoute("({id})")]
        public async Task<IActionResult> GetTodo([FromODataUri] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var todo = await _context.TodoList.FindAsync(id);

            if (todo == null)
            {
                return NotFound();
            }

            return Ok(todo);
        }

        [ODataRoute("({id})")]
        public async Task<IActionResult> Patch([FromODataUri] int id, [FromBody]Todo todo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != todo.Id)
            {
                return BadRequest();
            }

            var check = _context.TodoList.Where(item => item.Id == todo.Id).First();

            if (!check.Complete && todo.Complete)
            {
                check.MarkComplete();
            }

            check.Description = todo.Description;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [ODataRoute]
        public async Task<IActionResult> Post([FromBody] Todo todo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.TodoList.Add(todo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodo", new { id = todo.Id }, todo);
        }

        [ODataRoute("({id})")]
        public async Task<IActionResult> Delete([FromODataUri] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var todo = await _context.TodoList.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            _context.TodoList.Remove(todo);
            await _context.SaveChangesAsync();

            return Ok(todo);
        }

        private bool TodoExists(int id)
        {
            return _context.TodoList.Any(e => e.Id == id);
        }
    }
}