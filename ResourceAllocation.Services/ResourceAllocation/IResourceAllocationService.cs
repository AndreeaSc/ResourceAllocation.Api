using System.Collections.Generic;
using ResourceAllocation.Domain;

namespace ResourceAllocation.Services.ResourceAllocation
{
    public interface IResourceAllocationService
    {
        List<Designer> AllocateArtistsAlgorithm(List<Designer> designers, List<Artist> artist);
    }
}
