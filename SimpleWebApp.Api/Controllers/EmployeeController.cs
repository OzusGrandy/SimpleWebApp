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
        private readonly EmployeeManager _manager;

        public EmployeeController(EmployeeManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public async Task<PagingResult<EmployeeDto>> GetPage([FromQuery] GetEmployeePageDto getPage, CancellationToken cancellationToken)
        {
            var result = await _manager.GetPage(getPage, cancellationToken);

            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await _manager.Get(id, cancellationToken);

            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> Add([FromBody] EmployeeCreateDto data, CancellationToken cancellationToken)
        {
            var result = await _manager.Add(data, cancellationToken);
            return Created($"employee/{result.Id:N}", result);
        }

        [HttpPatch("{id:guid}")]
        public async Task<EmployeeDto> Update(
            [FromRoute] Guid id, 
            [FromBody] EmployeeCreateDto data, 
            CancellationToken cancellationToken)
        {
            return await _manager.Update(new EmployeeUpdateDto
            {
                Id = id,
                FirstName = data.FirstName,
                LastName = data.LastName,
                Birthday = data.Birthday
            }, 
            cancellationToken);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _manager.Delete(id, cancellationToken);
            return NoContent();
        }
    }

}
