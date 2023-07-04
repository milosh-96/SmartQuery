using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartQuery.Web.Data;
using SmartQuery.Web.Models;

namespace SmartQuery.Web.Areas.Api.Entries.Requests.Linker
{
    public record RemoveItemRequest : IRequest<bool>
    {
        public int ItemAId { get; set; }
        public int ItemBId { get; set; }
    }
    public class RemoveItemRequestHandler : IRequestHandler<RemoveItemRequest, bool>
    {
        private readonly SmartQueryDbContext _context;

        public RemoveItemRequestHandler(SmartQueryDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(RemoveItemRequest request, CancellationToken cancellationToken)
        {
            var pair = await _context.Set<EntryEntry>().Where(x =>
                    x.EntryId == request.ItemAId && x.RelatedEntryId == request.ItemBId ||
                    x.EntryId == request.ItemBId && x.RelatedEntryId == request.ItemAId)
                .ToListAsync();
            _context.Set<EntryEntry>().RemoveRange(pair);
               return await _context.SaveChangesAsync().ContinueWith(x =>
               {
                   if(x.Result == 0)
                   {
                       return false;
                   }
                   else
                   {
                       return true;
                   }
               });
        }
    }
}
