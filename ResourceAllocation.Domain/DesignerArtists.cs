using System;

namespace ResourceAllocation.Domain
{
    public class DesignerArtists
    {
        public Guid DesignerId { get; set; }
        public Guid ArtistId { get; set; }
        public Artist Artist { get; set; }
        public Designer Designer { get; set; }
    }
}