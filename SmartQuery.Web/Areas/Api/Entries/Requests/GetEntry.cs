using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartQuery.Web.Data;
using SmartQuery.Web.Models;

namespace SmartQuery.Web.Areas.Api.Entries.Requests
{
    public class GetEntryQuery : IRequest<Entry>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
    public class GetEntryQueryHandler : IRequestHandler<GetEntryQuery, Entry>
    {
        private readonly SmartQueryDbContext _context;

        public GetEntryQueryHandler(SmartQueryDbContext context)
        {
            _context = context;
        }

        public async Task<Entry> Handle(GetEntryQuery request, CancellationToken cancellationToken)
        {
            return await _context.Set<Entry>().Where(x => x.Id == request.Id).FirstOrDefaultAsync();
        }
    }



    public class GetEntryByNameQuery : IRequest<Entry>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
    public class GetEntryByNameQueryHandler : IRequestHandler<GetEntryByNameQuery, Entry>
    {
        private readonly SmartQueryDbContext _context;

        public GetEntryByNameQueryHandler(SmartQueryDbContext context)
        {
            _context = context;
        }

        public async Task<Entry> Handle(GetEntryByNameQuery request, CancellationToken cancellationToken)
        {
            return await _context.Set<Entry>()
                .Where(x => x.Name.ToLower() == request.Name.ToLower())
                .FirstOrDefaultAsync();
        }
    }
}
