using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartQuery.Web.Areas.Api.Entries.Requests.Linker;

namespace SmartQuery.Web.Areas.Api.Entries
{
    [Route("api/entries/[controller]")]
    [ApiController]
    public class LinkerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LinkerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{action=index}")]
        public IActionResult Index()
        {
            return new JsonResult(new { ok = "Linker" });
        }

        [HttpPost("{action}")]
        public async Task<IActionResult> Add([FromBody]AddItemRequest request)
        {
            var result = await _mediator.Send(request);
            if (result == null)
            {
                return new BadRequestObjectResult(new { message = "there was an error." });
            }
            request = new AddItemRequest();
            ModelState.Clear();
            return new JsonResult(result);
        }
        [HttpPost("{action}")]
        public async Task<IActionResult> Remove([FromBody]RemoveItemRequest request)
        {
            var result = await _mediator.Send(request);
            if (result == null)
            {
                return new BadRequestObjectResult(new { message = "there was an error." });
            }
            request = new RemoveItemRequest();
            ModelState.Clear();
            return new JsonResult(result);
        }
    }
}
