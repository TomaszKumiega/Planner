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
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public EventsController(ILogger<EventsController> logger, IUnitOfWorkFactory unitOfWorkFactory)
        {
            _logger = logger;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        [HttpGet]
        public IEnumerable<Event> GetAll()
        {
            using(var unitOfWork = _unitOfWorkFactory.GetUnitOfWork())
            {
                return unitOfWork.EventRepository.GetAll();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> Get(Guid id)
        {
            using(var unitOfWork = _unitOfWorkFactory.GetUnitOfWork())
            {
                try
                {
                    return unitOfWork.EventRepository.GetById(id);
                }
                catch(ArgumentNullException e)
                {
                    return NotFound();
                }
            }
        }
    }
}