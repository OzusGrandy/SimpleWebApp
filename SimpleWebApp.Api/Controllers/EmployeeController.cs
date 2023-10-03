using MediatR;
using Microsoft.AspNetCore.Mvc;
using SimpleWebApp.BusinessLogic;
using SimpleWebApp.BusinessLogic.Models;
using SimpleWebApp.CommonModels;

namespace SimpleWebApp.Api.Controllers
{
    [ApiController]
    [Route("employees")]
    public class EmployeeController : Controller
    {
        private readonly IMediator _mediator;

        public EmployeeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<PagingResult<EmployeeDto>> GetPage([FromQuery] GetEmployeePageDto getPage, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new BusinessLogic.Cqrs.GetPage.Query()
            {
                EmployeePage = getPage
            }, 
            cancellationToken);

            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new BusinessLogic.Cqrs.Get.Query() { Id = id }, cancellationToken);

            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> Add([FromBody] EmployeeCreateDto data, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new BusinessLogic.Cqrs.Create.Command() { EmployeeCreate = data }, cancellationToken);
            return Created($"employee/{result.Id:N}", result);
        }

        [HttpPatch("{id:guid}")]
        public async Task<EmployeeDto> Update(
            [FromRoute] Guid id, 
            [FromBody] EmployeeCreateDto data, 
            CancellationToken cancellationToken)
        {
            return await _mediator.Send(new BusinessLogic.Cqrs.Update.Command()
            {
                EmployeeUpdate = new EmployeeUpdateDto
                {
                    Id = id,
                    FirstName = data.FirstName,
                    LastName = data.LastName,
                    Birthday = data.Birthday
                }
            },
            cancellationToken);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new BusinessLogic.Cqrs.Delete.Command() { Id = id }, cancellationToken);
            return NoContent();
        }
    }

}
