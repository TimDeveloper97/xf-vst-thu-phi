using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using ThuPhi.Domain;

namespace ThuPhi.Model.Send
{
    class Login
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
