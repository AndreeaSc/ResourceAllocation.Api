using System;
using System.Collections.Generic;
using System.Text;

namespace ResourceAllocation.Domain
{
    public class Show : BaseEntity
    {
        public string Designer { get; set; }
        public DateTime Date { get; set; }
        public string ArtistsName { get; set; }
        public string Location { get; set; }
    }
}
