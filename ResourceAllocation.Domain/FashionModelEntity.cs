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
        public string EyesColor { get; set; }
        public string HairColor { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public string Gender { get; set; }
    }
}
