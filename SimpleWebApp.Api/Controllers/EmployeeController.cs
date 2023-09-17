using Microsoft.AspNetCore.Mvc;
using SimpleWebApp.Api.BuisnessLogic;
using SimpleWebApp.Api.Models;
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
        public PagingResult<EmployeeDto> GetPage([FromQuery] GetEmployeePageDto getPage)
        {
            var result = _manager.GetPage(getPage);

            return result;
        }

        [HttpGet("{id}")]
        public ActionResult<EmployeeDto> GetById([FromRoute] Guid id)
        {
            var result = _manager.Get(id);

            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        [HttpPost]
        public ActionResult<EmployeeDto> Add([FromBody] EmployeeCreateDto data)
        {
            var result = _manager.Add(data);
            return Created($"employee/{result.Id:N}", result);
        }

        [HttpPatch("{id:guid}")]
        public EmployeeDto Update([FromRoute] Guid id, [FromBody] EmployeeCreateDto data)
        {
            return _manager.Update(new EmployeeUpdateDto
            {
                Id = id,
                FirstName = data.FirstName,
                LastName = data.LastName,
                Birthday = data.Birthday
            });
        }

        [HttpDelete("{id:guid}")]
        public ActionResult Delete(Guid id)
        {
            _manager.Delete(id);
            return NoContent();
        }
    }

}
