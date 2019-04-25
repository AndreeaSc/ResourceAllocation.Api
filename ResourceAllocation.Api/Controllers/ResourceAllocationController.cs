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
        private readonly IResourceAllocationService _resourceAllocationService;

        private readonly IDesignersRepository _designerRepository;

        private readonly IArtistsRepository _artistsRepository;

        public ResourceAllocationController(IResourceAllocationService resourceAllocationService, IDesignersRepository designerRepository, IArtistsRepository artistsRepository)
        {
            _resourceAllocationService = resourceAllocationService;
            _designerRepository = designerRepository;
            _artistsRepository = artistsRepository;
        }

        [HttpGet]
        public List<Designer> Get()
        {
            var designers = _designerRepository.GetAll();
            var artists = _artistsRepository.GetAll();

            var result = _resourceAllocationService.AllocateArtistsAlgorithm(designers, artists);

            return result;
        }
    }
}
