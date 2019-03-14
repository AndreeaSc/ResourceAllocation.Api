using System;
using Microsoft.AspNetCore.Mvc;
using ResourceAllocation.Domain;
using ResourceAllocation.Services.FashionModels;

namespace ResourceAllocation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FashionModelsController : ControllerBase
    {
        private readonly IFashionModelsService _fashionModelsService;

        public FashionModelsController(IFashionModelsService fashionModelsService)
        {
            _fashionModelsService = fashionModelsService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = _fashionModelsService.GetAll();
            return Ok(result);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult Get(Guid id)
        {
            var result = _fashionModelsService.GetById(id);
            return Ok(result);
        }

        [HttpPut]
        public IActionResult Put(Artist entity)
        {
            _fashionModelsService.Add(entity);
            return Ok();
        }

        [HttpPatch]
        public IActionResult Patch(Artist entity)
        {
            _fashionModelsService.Update(entity);
            return Ok();
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult Delete(Guid id)
        {
            _fashionModelsService.Delete(id);
            return Ok();
        }
    }
}
