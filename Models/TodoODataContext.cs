using TodoOData.Models;
using Microsoft.EntityFrameworkCore;

namespace TodoOData.Models
{
    public class TodoODataContext : DbContext
    {
        public TodoODataContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Todo> TodoList { get; set; }
    }
}
