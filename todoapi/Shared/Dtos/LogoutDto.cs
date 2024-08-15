using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace todoapi.Shared.Dtos
{
    public class LogoutDto
    {
        public string? RefreshToken { get; set; }
    }
}