using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResourceAllocation.Domain
{
    public class Designer : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Mail { get; set; }
        public int nrOfArtistsNeeded { get; set; }
 
        public List<DesignerArtists> FavoriteArtists { get; set; } = new List<DesignerArtists>();

        [NotMapped]
        public List<DesignerArtists> AllocatedArtists { get; set; }

        [NotMapped]
        public int Score { get; set; }

        public DateTime DateTimeShow { get; set; }
        public String LocationShow { get; set; }
    }
}
