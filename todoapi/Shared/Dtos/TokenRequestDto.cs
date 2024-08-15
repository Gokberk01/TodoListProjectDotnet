using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace todoapi.Shared.Dtos
{
    public class TokenRequestDto
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}