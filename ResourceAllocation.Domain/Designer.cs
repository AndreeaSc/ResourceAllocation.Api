using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResourceAllocation.Domain
{
    public class Designer : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public List<DesignerArtists> FavoriteArtists { get; set; } = new List<DesignerArtists>();
    }
}
