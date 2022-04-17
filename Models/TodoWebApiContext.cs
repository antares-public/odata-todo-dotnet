using Microsoft.EntityFrameworkCore;
using TodoWebApi.Models;

namespace TodoWebApi.Models
{
    public class TodoWebApiContext : DbContext
    {
        public TodoWebApiContext(DbContextOptions options) :
            base(options)
        {
        }

        public DbSet<Todo> TodoList { get; set; }
    }
}
