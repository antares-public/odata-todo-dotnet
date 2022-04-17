using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TodoWebApi.Models;

using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoWebApi.Controllers
{
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly TodoWebApiContext _context;

        public TodoController(TodoWebApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Todo> GetAll()
        {
            return _context.TodoList.ToList();
        }

        [HttpPost]
        public IActionResult Create([FromBody] Todo item)
        {
            if (item == null) return BadRequest();

            _context.TodoList.Add(item);
            _context.SaveChanges();
            return Created($"api/todo/{item.Id}", item);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (_context.TodoList.FirstOrDefault(item => item.Id == id) == null) return NotFound();

            _context.TodoList.FirstOrDefault(item => item.Id == id);
            _context.SaveChanges();
            return new NoContentResult();
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Todo item)
        {
            if (item == null || item.Id != id) return BadRequest();

            var task = _context.TodoList.FirstOrDefault(t => t.Id == id);
            if (task == null) return NotFound();

            task.isCompleted = item.isCompleted;
            task.Name = item.Name;

            _context.TodoList.Update(task);
            _context.SaveChanges();

            return new NoContent();
        }
    }
}
