using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SmartQuery.Web.Data;
using SmartQuery.Web.Models;

namespace SmartQuery.Web.Pages.Entries.Api
{
    public class GetRelatedEntriesModel : PageModel
    {
        private readonly IMediator _mediator;

        public GetRelatedEntriesModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [FromQuery]
        public int EntryId { get; set; } = 0;
        public async Task<IActionResult> OnGet()
        {
            if (EntryId < 1) { return new JsonResult("no data"); };
            List<Entry> items = new List<Entry>();
            List<Entry> result = await _mediator.Send(new GetRelatedEntriesQuery() { Id = EntryId });
            if (result != null && result.Count > 0)
            {
                items.AddRange(result);
            }
            return new JsonResult(items);
        }

        public class GetRelatedEntriesQuery : IRequest<List<Entry>>
        {
            public int Id { get; set; }
        }
        public class GetRelatedEntriesQueryHandler : IRequestHandler<GetRelatedEntriesQuery, List<Entry>>
        {
            private readonly SmartQueryDbContext _context;

            public GetRelatedEntriesQueryHandler(SmartQueryDbContext context)
            {
                _context = context;
            }

            public async Task<List<Entry>> Handle(GetRelatedEntriesQuery request, CancellationToken cancellationToken)
            {
                var item = await _context.Set<Entry>()
                    .Where(x => x.Id == request.Id)
                    .Include(e => e.RelatedEntries).ThenInclude(x=>x.RelatedEntry)
                    .FirstOrDefaultAsync();
                if(item == null) { throw new InvalidOperationException(); }
                return item.RelatedEntries.Select(r => r.RelatedEntry).ToList();
            }
        }
    }
}
