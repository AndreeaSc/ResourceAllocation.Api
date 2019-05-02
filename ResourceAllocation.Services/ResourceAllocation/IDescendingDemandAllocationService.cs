using ResourceAllocation.Domain;

namespace ResourceAllocation.Services.ResourceAllocation
{
    public interface IDescendingDemandAllocationService
    {
        AlgorithmResult Execute();
    }
}
