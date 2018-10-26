using System;

namespace ResourceAllocation.Domain
{
    public class FashionModelEntity : BaseEntity
    {
        public string Name { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public int BreastSize { get; set; }
        public int WaistSize { get; set; }
        public int HipsSize { get; set; }
        public EyesColorType EyesColor { get; set; }
        public HairColorType HairColor { get; set; }
    }
}
