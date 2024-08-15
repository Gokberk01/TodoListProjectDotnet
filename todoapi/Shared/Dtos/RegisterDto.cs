using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace todoapi.Dtos
{
    //Datas we will send from frontend
    public class RegisterDto
    {
        public string  UserName { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public string UserPassword { get; set; } = null!;

    }
}