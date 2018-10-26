﻿using System;
using System.Collections.Generic;
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
        public ActionResult<IEnumerable<FashionModelEntity>> Get()
        {
            var result = _fashionModelsService.GetAll();
            return Ok(result);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public ActionResult<FashionModelEntity> Get(Guid id)
        {
            var result = _fashionModelsService.GetById(id);
            return Ok(result);
        }

        [HttpPut]
        public void Put(FashionModelEntity entity)
        {
            _fashionModelsService.Add(entity);
        }

        [HttpPatch]
        public void Patch(FashionModelEntity entity)
        {
            _fashionModelsService.Update(entity);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public void Delete(Guid id)
        {
            _fashionModelsService.Delete(id);
        }
    }
}