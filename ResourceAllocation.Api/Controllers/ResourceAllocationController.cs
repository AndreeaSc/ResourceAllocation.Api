using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ResourceAllocation.Domain;
using ResourceAllocation.Services.ResourceAllocation;

namespace ResourceAllocation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourceAllocationController : ControllerBase
    {
        private readonly IResourceAllocationService _resourceAllocationService;

        public ResourceAllocationController(IResourceAllocationService resourceAllocationService)
        {
            _resourceAllocationService = resourceAllocationService;
        }

        [HttpGet]
        public List<Designer> Get()
        {
            var result = _resourceAllocationService.ExecuteAlgorithm();
            return result;
        }
    }
}
