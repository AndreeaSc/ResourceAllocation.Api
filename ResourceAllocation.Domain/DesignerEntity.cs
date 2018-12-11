using System;
using System.Collections.Generic;
using System.Text;

namespace ResourceAllocation.Domain
{
    public class DesignerEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
    }
}
