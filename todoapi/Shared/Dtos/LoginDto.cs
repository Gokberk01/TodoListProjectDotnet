using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace todoapi.Dtos
{
    public class LoginDto
    {
        public string UserEmail { get; set; } = null!;
        public string UserPassword { get; set; } = null!;
    }
}