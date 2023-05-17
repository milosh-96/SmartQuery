using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SmartQuery.Web.Data;
using SmartQuery.Web.Models;

namespace SmartQuery.Web.Pages.Adjectives.Api
{
    public class ListByNameModel : PageModel
    {
        private readonly IMediator _mediator;

        public ListByNameModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty(SupportsGet =true)]
        public string? Query { get; set; }
        public async Task<IActionResult> OnGet()
        {
            if(Query == null) { return new JsonResult("no data"); };
            List<Adjective> adjectives = new List<Adjective>();
            List<Adjective> result = await _mediator.Send(new ListByNameQuery() { Name = Query });
            if(result != null && result.Count > 0)
            {
                adjectives.AddRange(result);
            }
            return new JsonResult(adjectives);
        }

        public class ListByNameQuery : IRequest<List<Adjective>>
        {
            public string Name { get; set; }
        }
        public class ListByNameQueryHandler : IRequestHandler<ListByNameQuery, List<Adjective>>
        {
            private readonly SmartQueryDbContext _context;

            public ListByNameQueryHandler(SmartQueryDbContext context)
            {
                _context = context;
            }

            public async Task<List<Adjective>> Handle(ListByNameQuery request, CancellationToken cancellationToken)
            {
                return await _context.Set<Adjective>().Where(x=>x.Name.ToLower().StartsWith(request.Name.ToLower())).ToListAsync();
            }
        }
    }
}
