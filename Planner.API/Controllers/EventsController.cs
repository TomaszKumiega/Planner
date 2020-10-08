using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Planner.Model;
using Planner.Model.Repositories;

namespace Planner.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly ILogger<EventsController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public EventsController(ILogger<EventsController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        ~EventsController()
        {
            _unitOfWork.Dispose();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetAll()
        {
            var events = await Task.Run(() => _unitOfWork.EventRepository.GetAll());
            
            if(events?.Any() != true) return NotFound();
            
            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> Get(Guid id)
        {
            var @event = await Task.Run(() => _unitOfWork.EventRepository.GetById(id));

            if (@event == null) return NotFound();

            return Ok(@event);
        }

        [HttpPost]
        public async Task<ActionResult<Event>> AddEvent([FromBody] Event @event)
        {
            await Task.Run(() => _unitOfWork.EventRepository.Add(@event));
            _unitOfWork.SaveChanges();

            return Created(HttpContext.Request.GetDisplayUrl(), @event);
        }

        [HttpDelete("/remove")]
        public async Task<ActionResult<Event>> RemoveEvent([FromBody] Event @event)
        {
            await Task.Run(() => _unitOfWork.EventRepository.Remove(@event));
            _unitOfWork.SaveChanges();

            return Ok(@event);
        }
    }
}