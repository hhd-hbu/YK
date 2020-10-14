using System;
using System.Collections.Generic;
using System.Text;

namespace YK.Util.Model
{
    public class JWTPayload
    {
        public string UserId { get; set; }
        public DateTime Expire { get; set; }
    }
}
