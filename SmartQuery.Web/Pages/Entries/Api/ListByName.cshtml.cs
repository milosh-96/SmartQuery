using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SmartQuery.Web.Data;
using SmartQuery.Web.Models;

namespace SmartQuery.Web.Pages.Entries.Api
{
    public class ListByNameModel : PageModel
    {
        private readonly IMediator _mediator;

        public ListByNameModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [FromQuery]
        public string? Query { get; set; }
        public async Task<IActionResult> OnGet()
        {
            if(Query == null) { return new JsonResult("no data"); };
            List<Entry> items = new List<Entry>();
            List<Entry> result = await _mediator.Send(new ListByNameQuery() { Name = Query });
            if(result != null && result.Count > 0)
            {
                items.AddRange(result);
            }
            return new JsonResult(items);
        }

        public class ListByNameQuery : IRequest<List<Entry>>
        {
            public string Name { get; set; }
        }
        public class ListByNameQueryHandler : IRequestHandler<ListByNameQuery, List<Entry>>
        {
            private readonly SmartQueryDbContext _context;

            public ListByNameQueryHandler(SmartQueryDbContext context)
            {
                _context = context;
            }

            public async Task<List<Entry>> Handle(ListByNameQuery request, CancellationToken cancellationToken)
            {
                return await _context.Set<Entry>().Where(x=>x.Name.ToLower().Contains(request.Name.ToLower())).ToListAsync();
            }
        }
    }
}
