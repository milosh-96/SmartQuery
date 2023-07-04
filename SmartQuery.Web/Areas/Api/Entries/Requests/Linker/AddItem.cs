using MediatR;
using SmartQuery.Web.Data;
using SmartQuery.Web.Models;

namespace SmartQuery.Web.Areas.Api.Entries.Requests.Linker
{
    public record AddItemRequest : IRequest<List<EntryEntry>>
    {
        public int TargetId { get; set; }
        public int LinkingEntryId { get; set; }
    }
    public class AddItemRequestHandler : IRequestHandler<AddItemRequest, List<EntryEntry>>
    {
        private readonly SmartQueryDbContext _context;

        public AddItemRequestHandler(SmartQueryDbContext context)
        {
            _context = context;
        }
        public async Task<List<EntryEntry>> Handle(AddItemRequest request, CancellationToken cancellationToken)
        {
            List<EntryEntry> relatedEntries = new List<EntryEntry>();
            if (request.TargetId > 0 && request.LinkingEntryId > 0)
            {
                relatedEntries.Add(
                    new EntryEntry()
                    {
                        EntryId = request.TargetId,
                        RelatedEntryId = request.LinkingEntryId
                    });
                relatedEntries.Add(
                    new EntryEntry()
                    {
                        EntryId = request.LinkingEntryId,
                        RelatedEntryId = request.TargetId
                    }
                    );
                _context.Set<EntryEntry>().AddRange(relatedEntries);
                await _context.SaveChangesAsync();
                return relatedEntries;
            }
            return null;
        }
    }
}
