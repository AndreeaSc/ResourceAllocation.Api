using System.Collections.Generic;

namespace ResourceAllocation.Domain
{
    public class DesignerEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public List<FashionModelEntity> SelectedModels { get; set; }
    }
}
