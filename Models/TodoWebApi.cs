using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TodoWebApi.Models
{
    public class Todo
    {
        public Todo()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("Name", TypeName = "text")]
        [StringLengthAttribute(100)]
        public string Name { get; set; }

        [Column("Content", TypeName = "tinyint")]
        public bool isCompleted { get; set; }
    }
}
