using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nancy.Json;
using Planner.Model;
using Planner.Model.Repositories;

namespace Planner.API.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        public EventsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            return Ok(await Task.Run(() => _unitOfWork.EventRepository.GetAll()));
        }

        // GET: api/Events/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(Guid id)
        {
            var @event = await Task.Run(() => _unitOfWork.EventRepository.Find(x => x.Id == id));

            if (@event == null)
            {
                return NotFound();
            }

            return @event;
        }

        // PUT: api/Events/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(Guid id, [FromBody] Object ev)
        {
            var jsonString = ev.ToString();
            var @event = new JavaScriptSerializer().Deserialize<Event>(jsonString);

            if (id != @event.Id)
            {
                return BadRequest();
            }

            _unitOfWork.EventRepository.Update(@event);

            try
            {
                await Task.Run(() => _unitOfWork.SaveChanges());
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
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

        // POST: api/Events
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Event>> PostEvent([FromBody] Object ev)
        {
            var jsonString = ev.ToString();
            var @event = new JavaScriptSerializer().Deserialize<Event>(jsonString);

            _unitOfWork.EventRepository.Add(@event);
            await Task.Run(() => _unitOfWork.SaveChanges());

            return CreatedAtAction("GetEvent", new { id = @event.Id }, @event);
        }

        // DELETE: api/Events/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Event>> DeleteEvent(Guid id)
        {
            var @event = await Task.Run(() => _unitOfWork.EventRepository.Find(x => x.Id == id));
            if (@event == null)
            {
                return NotFound();
            }

            _unitOfWork.EventRepository.Remove(@event);
            await Task.Run(() => _unitOfWork.SaveChanges());

            return @event;
        }

        private bool EventExists(Guid id)
        {
            return _unitOfWork.EventRepository.Find(x => x.Id == id) != null;
        }
    }
}
