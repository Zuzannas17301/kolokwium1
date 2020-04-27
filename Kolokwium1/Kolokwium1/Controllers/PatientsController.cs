using System.Threading.Tasks;
using Kolokwium1.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kolokwium1.Controllers
{
    [ApiController]
    [Route("api/patients")]
    public class PatientsController :ControllerBase
    {
        private readonly IDeleteService _dbService;

        public PatientsController(IDeleteService dbService)
        {
            _dbService = dbService;
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePatient(int id)
        {
            var response = _dbService.DeletePatient(id);
            if (response == null)
            {
                return BadRequest();
            }

            return Ok();

        }
        
    }
}