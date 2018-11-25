using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ResourceAllocation.Domain;
using ResourceAllocation.Services.Show;

namespace ResourceAllocation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowsController : ControllerBase
    {
        private readonly IShowsService _showsService;

        public ShowsController(IShowsService showsService)
        {
            _showsService = showsService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ShowEntity>> Get()
        {
            var result = _showsService.GetAll();
            return Ok(result);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public ActionResult<ShowEntity> Get(Guid id)
        {
            var result = _showsService.GetById(id);
            return Ok(result);
        }

        [HttpPut]
        public void Put(ShowEntity entity)
        {
            _showsService.Add(entity);
        }

        [HttpPatch]
        public void Patch(ShowEntity entity)
        {
            _showsService.Update(entity);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public void Delete(Guid id)
        {
        }
    }
}