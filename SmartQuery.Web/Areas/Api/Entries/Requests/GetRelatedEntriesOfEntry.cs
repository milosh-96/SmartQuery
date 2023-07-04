using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartQuery.Web.Data;
using SmartQuery.Web.Models;

namespace SmartQuery.Web.Areas.Api.Entries.Requests
{
    public class GetRelatedEntriesOfEntryQuery : IRequest<List<Entry>>
    {
        public int Id { get; set; }
    }
    public class GetRelatedEntriesOfEntryHandler : IRequestHandler<GetRelatedEntriesOfEntryQuery, List<Entry>>
    {
        private readonly SmartQueryDbContext _context;

        public GetRelatedEntriesOfEntryHandler(SmartQueryDbContext context)
        {
            _context = context;
        }

        public async Task<List<Entry>> Handle(GetRelatedEntriesOfEntryQuery request, CancellationToken cancellationToken)
        {
            var item = await _context.Set<Entry>()
                .Where(x => x.Id == request.Id)
                .Include(e => e.RelatedEntries).ThenInclude(x => x.RelatedEntry)
                .FirstOrDefaultAsync();
            if (item == null) { throw new InvalidOperationException(); }
            return item.RelatedEntries.Select(r => r.RelatedEntry).ToList();
        }
    }
}
