using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
