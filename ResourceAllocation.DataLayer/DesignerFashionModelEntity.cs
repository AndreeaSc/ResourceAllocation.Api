using System;
using ResourceAllocation.Domain;

namespace ResourceAllocation.DataLayer
{
    public class DesignerFashionModelEntity
    {
        public Guid DesignerEntityId { get; set; }
        public DesignerEntity DesignerEntity { get; set; }

        public Guid FashionModelEntityId{ get; set; }
        public FashionModelEntity FashionModelEntity { get; set; }
    }
}
