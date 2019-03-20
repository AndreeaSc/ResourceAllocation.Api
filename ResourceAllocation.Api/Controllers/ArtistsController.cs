using System;
using Microsoft.AspNetCore.Mvc;
using ResourceAllocation.Domain;
using ResourceAllocation.Services.Artists;

namespace ResourceAllocation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private readonly IArtistsService _artistsService;

        public ArtistsController(IArtistsService artistsService)
        {
            _artistsService = artistsService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = _artistsService.GetAll();
            return Ok(result);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult Get(Guid id)
        {
            var result = _artistsService.GetById(id);
            return Ok(result);
        }

        [HttpPut]
        public IActionResult Put(Artist entity)
        {
             _artistsService.Add(entity);
            return Ok();
        }

        [HttpPatch]
        public IActionResult Patch(Artist entity)
        {
            _artistsService.Update(entity);
            return Ok();
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult Delete(Guid id)
        {
            _artistsService.Delete(id);
            return Ok();
        }
    }
}
