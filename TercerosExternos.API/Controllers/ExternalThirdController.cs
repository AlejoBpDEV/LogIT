using Microsoft.AspNetCore.Mvc;
using TercerosExternos.Application.Queries;

namespace TercerosExternos.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TercerosController : ApiBaseController
    {
        [HttpGet]
        [Route("ConsultarTercerosExternos")]
        public async Task<IActionResult> ConsultarTercerosExternos([FromQuery] GetExternalThirdListQuery query)
        {
            var result = await Mediator.Send(query);
            return Ok(result);
        }
    }
}
