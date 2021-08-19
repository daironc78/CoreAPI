using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace webAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MiControllerBase : ControllerBase
    {
        private IMediator mediator;
        protected IMediator _mediator => mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    }
}
