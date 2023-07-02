using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartQuery.Web.Data;
using SmartQuery.Web.Models;
using System.Text.Json.Nodes;

namespace SmartQuery.Web.Pages.Entries.Api.Linker
{
    [IgnoreAntiforgeryToken]

    public class AddModel : PageModel
    {
        private readonly IMediator _mediator;

        public AddModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public record Command : IRequest<List<EntryEntry>>
        {
            public int TargetId { get; set; }
            public int LinkingEntryId { get; set; }
        }
        public class CommandHandler : IRequestHandler<Command,List<EntryEntry>>
        {
            private readonly SmartQueryDbContext _context;

            public CommandHandler(SmartQueryDbContext context)
            {
                _context = context;
            }
            public async Task<List<EntryEntry>> Handle(Command request, CancellationToken cancellationToken)
            {
                List<EntryEntry> relatedEntries = new List<EntryEntry>();
                if(request.TargetId > 0 && request.LinkingEntryId > 0)
                {
                    relatedEntries.Add(
                        new EntryEntry() {
                            EntryId = request.TargetId,
                            RelatedEntryId = request.LinkingEntryId
                        });
                    relatedEntries.Add(
                        new EntryEntry() {
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

        [BindProperty]
        public Command Data { get; set; } = new Command();
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var result = await _mediator.Send(Data);
            if (result == null) {
                return new BadRequestObjectResult(new { message = "there was an error." });
            }
                Data = new Command();
                ModelState.Clear();
            return new JsonResult(result);

        }
    }
}

