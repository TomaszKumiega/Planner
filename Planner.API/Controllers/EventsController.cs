using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
            return Ok(await Task.Run(() => _unitOfWork.EventRepository.GetAll()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> Get(Guid id)
        {
            return Ok(await Task.Run(() => _unitOfWork.EventRepository.GetById(id)));
        }

        [HttpPost]
        public async Task<ActionResult<Event>> AddEvent([FromBody] Event @event)
        {
            await Task.Run(() => _unitOfWork.EventRepository.Add(@event));
            _unitOfWork.SaveChanges();

            return Ok(@event);
        }
    }
}