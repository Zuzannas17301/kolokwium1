using System.Threading.Tasks;
using Kolokwium1.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kolokwium1.Controllers
{
    [ApiController]
    [Route("api/medicaments")]
    public class MedicamentController : ControllerBase
    {
        private readonly IDbService _dbService;

        public MedicamentController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMedicament(int id)
        {
            var response = _dbService.GetMedicament(id);
            if (response == null)
            {
                return BadRequest();
            }

            return Ok();
        }
        
    }
}