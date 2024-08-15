using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace todoapi.Shared.Dtos
{
    public class TodoDto
    {
        public string context { get; set; } = null!;
        public Boolean IstoDoDone { get; set; }
        public Boolean IsDeleted { get; set; }

        
    }
}