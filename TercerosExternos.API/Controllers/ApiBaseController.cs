using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TercerosExternos.API.Controllers
{
    public abstract class ApiBaseController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    }
}
