using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.UseCases;
using Web.Api.Models.Request;
using Web.Api.Presenters;


namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly RequestContext _context;

        public RequestController(RequestContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<RequestRequest>> GetRequestList()
        {
            return await _context.Request.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Models.Request.RequestRequest request)
        {
            _context.Request.Add(request);
            await _context.SaveChangesAsync();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromBody] Models.Request.RequestRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            _context.Entry(request).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromBody] Models.Request.RequestRequest request)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            return request;
        }

        private bool RequestExists(long id)
        {
            return _context.RequestItems.Any(e => e.Id == id);
        }
    }
}