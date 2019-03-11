using System;
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

        [HttpGet("{id:Guid}/algorithm")]
        public IActionResult GetResultedModelsById(Guid id)
        {
            var result = _designersService.GetById(id);
            return Ok(result);
        }

        [HttpPut]
        public IActionResult Put(DesignerEntity entity)
        {
            _designersService.Add(entity);
            return Ok();
        }

        [HttpPatch]
        public IActionResult Patch(DesignerEntity entity)
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