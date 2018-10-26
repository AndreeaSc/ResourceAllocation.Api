using System;

namespace ResourceAllocation.Domain
{
   public class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
