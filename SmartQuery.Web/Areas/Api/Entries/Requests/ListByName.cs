using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartQuery.Web.Data;
using SmartQuery.Web.Models;

namespace SmartQuery.Web.Areas.Api.Entries.Requests
{
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
            return await _context.Set<Entry>().Where(x => x.Name.ToLower().Contains(request.Name.ToLower())).ToListAsync();
        }
    }
}
