using System.Collections.Generic;

namespace ResourceAllocation.Domain
{
    public class AlgorithmResult
    {
        public List<Designer> Designers { get; set; }
        public int Score { get; set; }
        public long TimeExecuted { get; set; }
    }
}
