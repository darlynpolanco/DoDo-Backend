using Microsoft.AspNetCore.Mvc;
using MediatR;
using DoDo.Features.Todo;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace DoDo.Features.Todo
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController: ControllerBase
    {
        private readonly IMediator _mediator;

        public TodoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/todo/all
        [HttpGet("all")]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new Get.Query());
            return Ok(result);
        }

        // GET: api/todo/{id}
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetById.Query(id));
            return Ok(result);
        }


        // POST: api/todo
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Create.Command command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id, userId = result.UsuarioId }, result);
        }

        // PUT: api/todo
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Update.Command command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // DELETE: api/todo/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new Delete.Command(id));
            return NoContent();
        }

        // GET: api/todo/priority?priority=Alta
        [Authorize]
        [HttpGet("priority")]
        public async Task<IActionResult> GetByPriority([FromQuery] string priority)
        {
            var result = await _mediator.Send(new GetByPriority.Query(priority));
            return Ok(result);
        }

        // GET: api/todo/search?text=estudiar
        [Authorize]
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string text)
        {
            var result = await _mediator.Send(new SearchByText.Query(text));
            return Ok(result);
        }

        // PATCH: api/todo/{id}/complete
        [HttpPatch("{id}/complete")]
        public async Task<IActionResult> MarkAsCompleted(int id)
        {
            await _mediator.Send(new MarkAsCompleted.Command(id));
            return NoContent();
        }

        // PATCH: api/todo/{id}/uncomplete
        [HttpPatch("{id}/uncomplete")]
        public async Task<IActionResult> MarkAsUncompleted(int id)
        {
            await _mediator.Send(new MarkAsUncompleted.Command(id));
            return NoContent();
        }
    }
}