using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartQuery.Web.Areas.Api.Entries.Requests;
using SmartQuery.Web.Models;

namespace SmartQuery.Web.Areas.Api.Entries
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EntriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{action}")]
        public IActionResult Index()
        {
            return new JsonResult(new { ok = "Ok" });
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Single([FromRoute]GetEntryQuery request)
        {
            Entry? entry = null;
            await _mediator.Send(request).ContinueWith(x =>
            {
                if (x.Result != null)
                {
                    entry = x.Result;
                   

                }
            });
            if(entry != null)
            {
                return new JsonResult(entry);
            }
            return new NotFoundObjectResult(new { message = "not found" });

        }

        [HttpGet("{id:int}/{action}")]

        public async Task<IActionResult> GetRelatedEntries([FromRoute] GetRelatedEntriesOfEntryQuery request)
        {
            var result = new List<Entry>();
            await _mediator.Send(request).ContinueWith(x =>
            {
                if (x.Result != null && x.Result.Count > 0)
                {
                    result.AddRange(x.Result);
                }
            });
            return new JsonResult(result);
        }

        [HttpGet("{action}")]
        public async Task<IActionResult> List([FromQuery]ListByNameQuery request) {
            var result = new List<Entry>();
            await _mediator.Send(request).ContinueWith(x =>
            {
                if(x.Result != null && x.Result.Count > 0)
                {
                    result.AddRange(x.Result);
                }
            });
            return new JsonResult(result);
        }
    }
}
