using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ResourceAllocation.DataLayer.Artists;
using ResourceAllocation.DataLayer.Designers;
using ResourceAllocation.Domain;
using ResourceAllocation.Services.ResourceAllocation;

namespace ResourceAllocation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourceAllocationController : ControllerBase
    {
        private readonly IAdjustedWinnerAllocationService _adjustedWinnerAllocationService;
        private readonly IDescendingDemandAllocationService _descendingDemandAllocationService;

        public ResourceAllocationController(IAdjustedWinnerAllocationService adjustedWinnerAllocationService, IDescendingDemandAllocationService descendingDemandAllocationService)
        {
            _adjustedWinnerAllocationService = adjustedWinnerAllocationService;
            _descendingDemandAllocationService = descendingDemandAllocationService;
        }

        [HttpGet]
        [Route("descending-demand")]
        public AlgorithmResult GetDescendingDemand()
        {
            var result = _descendingDemandAllocationService.Execute();

            return result;
        }

        [HttpGet]
        [Route("adjusted-winner")]
        public AlgorithmResult GetAdjustedWinner()
        {
            var result = _adjustedWinnerAllocationService.Execute();

            return result;
        }
    }
}
