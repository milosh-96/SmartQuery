using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SmartQuery.Web.Data;
using SmartQuery.Web.Models;

namespace SmartQuery.Web.Pages.Entries.Api
{
    public class SingleModel : PageModel
    {
        private readonly IMediator _mediator;

        public SingleModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [FromQuery]
        public int Id { get; set; }
        [FromQuery]
        public string? Name { get; set; }
        public async Task<IActionResult> OnGet()
        {
            if (Id < 1 && Name == null) {
                return new BadRequestObjectResult(new { message = "Invalid parameters. Please use either Entry ID or Name parameter." });
            }
            Entry? entry = new Entry();
            IRequest<Entry>? getEntryQuery;
            if(Name != null)
            {
                getEntryQuery = new GetEntryByNameQuery() { Name = Name };
            }
            else
            {
                getEntryQuery = new GetEntryQuery() { Id = Id };

            }
            Entry? result = await _mediator.Send(getEntryQuery);
            if (result == null) {
                return new NotFoundObjectResult(new { message = "Entry doesn't exist." });
            }
            entry = result;
            return new JsonResult(entry);
        }

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
                return await _context.Set<Entry>().Where(x => x.Name.ToLower() == request.Name.ToLower()).FirstOrDefaultAsync();
            }
        }


    }
}
