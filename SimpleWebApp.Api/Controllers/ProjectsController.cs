using MediatR;
using Microsoft.AspNetCore.Mvc;
using SimpleWebApp.BusinessLogic.Project;
using SimpleWebApp.Middleware.CustomExceptions;

namespace SimpleWebApp.Api.Controllers
{
    [ApiController]
    [Route("projects")]
    public class ProjectsController : Controller
    {
        private readonly IMediator _mediator;

        public ProjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<List<Project>> GetAllProjects(
            [FromQuery] BusinessLogic.Project.GetAllProjects.Query query,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);

            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _mediator.Send(new BusinessLogic.Project.Get.Query() { Id = id }, cancellationToken);

                return result;
            }
            catch
            {
                throw new NoFoundException();
            }
        }

        [HttpPost]
        public async Task<ActionResult<Project>> Add([FromBody] ProjectCreate data, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new BusinessLogic.Project.Create.Command() { ProjectCreate = data }, cancellationToken);
            return Created($"projects/{result.Id:N}", result);
        }

        [HttpPatch("{id:guid}")]
        public async Task<Project> Update(
            [FromRoute] Guid id,
            [FromBody] ProjectCreate data,
            CancellationToken cancellationToken)
        {
            return await _mediator.Send(new BusinessLogic.Project.Update.Command()
            {
                ProjectUpdate = new ProjectUpdate
                {
                    Id = id,
                    Name = data.Name,
                    Description = data.Description,
                }
            },
            cancellationToken);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new BusinessLogic.Project.Delete.Command() { Id = id }, cancellationToken);
            return NoContent();
        }
    }
}
