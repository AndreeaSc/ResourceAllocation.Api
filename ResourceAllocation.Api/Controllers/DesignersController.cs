using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ResourceAllocation.Domain;
using ResourceAllocation.Services.Designers;


namespace ResourceAllocation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesignersController : ControllerBase
    {
        private readonly IDesignersService _designersService;

        public DesignersController(IDesignersService designersService)
        {
            _designersService = designersService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = _designersService.GetAll();
            return Ok(result);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult Get(Guid id)
        {
            var result = _designersService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        [Route("{id:Guid}/set-favourite-models")]
        public IActionResult SetFavouritesArtists(Guid id, [FromBody]List<Guid> ArtistIds)
        {
            _designersService.SetFavouriteArtists(id, ArtistIds);
            return Ok();
        }

        [HttpPut]
        public IActionResult Put(Designer entity)
        {
            _designersService.Add(entity);
            return Ok();
        }

        [HttpPatch]
        public IActionResult Patch(Designer entity)
        {
           _designersService.Update(entity);
            return Ok();
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult Delete(Guid id)
        {
            _designersService.Delete(id);
            return Ok();
        }
    }
}