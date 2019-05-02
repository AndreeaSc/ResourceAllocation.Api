using ResourceAllocation.Domain;

namespace ResourceAllocation.Services.ResourceAllocation
{
    public interface IAdjustedWinnerAllocationService
    {
        AlgorithmResult Execute();
    }
}
