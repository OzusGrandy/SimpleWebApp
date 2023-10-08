using MediatR;
using Microsoft.AspNetCore.Mvc;
using SimpleWebApp.BusinessLogic.Employee;
using SimpleWebApp.CommonModels;
using SimpleWebApp.Middleware.CustomExceptions;

namespace SimpleWebApp.Api.Controllers
{
    [ApiController]
    [Route("employees")]
    public class EmployeesController : Controller
    {
        private readonly IMediator _mediator;

        public EmployeesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<PagingResult<Employee>> GetPage(
            [FromQuery] BusinessLogic.Employee.GetPage.Query query, 
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);

            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _mediator.Send(new BusinessLogic.Employee.Get.Query() { Id = id }, cancellationToken);

                return result;
            }
            catch
            {
                throw new NoFoundException();
            }

        }

        [HttpPost]
        public async Task<ActionResult<Employee>> Add([FromBody] EmployeeCreate data, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new BusinessLogic.Employee.Create.Command() { EmployeeCreate = data }, cancellationToken);
            return Created($"employee/{result.Id:N}", result);
        }

        [HttpPatch("{id:guid}")]
        public async Task<Employee> Update(
            [FromRoute] Guid id, 
            [FromBody] EmployeeCreate data, 
            CancellationToken cancellationToken)
        {
            return await _mediator.Send(new BusinessLogic.Employee.Update.Command()
            {
                EmployeeUpdate = new EmployeeUpdate
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
            await _mediator.Send(new BusinessLogic.Employee.Delete.Command() { Id = id }, cancellationToken);
            return NoContent();
        }
    }

}
