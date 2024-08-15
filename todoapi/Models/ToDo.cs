using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace todoapi.Models
{
    public class ToDo
    {
        [Key]
        public int ToDoID { get; set; }

        public string ToDoContent { get; set; } = null!;

        public bool IstoDoDone { get; set; }

        public bool IsDeleted { get; set; }
    }


}