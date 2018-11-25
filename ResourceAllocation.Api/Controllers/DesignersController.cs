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
        public ActionResult<IEnumerable<DesignerEntity>> Get()
        {
            var result = _designersService.GetAll();
            return Ok(result);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public ActionResult<DesignerEntity> Get(Guid id)
        {
            var result = _designersService.GetById(id);
            return Ok(result);
        }

        [HttpPut]
        public void Put(DesignerEntity entity)
        {
            _designersService.Add(entity);
        }

        [HttpPatch]
        public void Patch(DesignerEntity entity)
        {
            _designersService.Update(entity);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public void Delete(Guid id)
        {
            _designersService.Delete(id);
        }
    }
}