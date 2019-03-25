using System.Collections.Generic;
using ResourceAllocation.Domain;

namespace ResourceAllocation.Services.ResourceAllocation
{
    public interface IResourceAllocationService
    {
        List<Designer> ExecuteAlgorithm();
    }
}
