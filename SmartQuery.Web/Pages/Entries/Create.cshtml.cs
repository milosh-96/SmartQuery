using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SmartQuery.Web.Data;
using SmartQuery.Web.Models;

namespace SmartQuery.Web.Pages.Entries
{
    public class CreateModel : PageModel
    {
        private readonly IMediator _mediator;

        public CreateModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public Command Data { get; set; } = new Command();
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
             var result = await _mediator.Send(Data);
            if(result == 1)
            {
                Data = new Command();
                ModelState.Clear();
            }
            return RedirectToPage("/Entries/Create");

        }
        public record Command : IRequest<int>
        {
            public string Name { get; init; }
            public string Description { get; init; }
            public string Adjectives { get; init; }
            public string RelatedTo { get; init; }
        }
        public class CommandHandler : IRequestHandler<Command, int>
        {
            private readonly SmartQueryDbContext _context;

            public CommandHandler(SmartQueryDbContext context)
            {
                _context = context;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                Entry entry = new Entry() {
                    Name = request.Name,
                    Description = request.Description,
                    Slug = new Slugify.SlugHelper().GenerateSlug(request.Name),
                    CreatedAt = DateTimeOffset.UtcNow
                };
                // parse adjectives input and try to find it in the adjectives table and then link //
                foreach(var item in request.Adjectives.Split(","))
                {
                    var adjective = await _context.Set<Adjective>().FirstOrDefaultAsync(x => x.Name == item);
                    if (adjective != null)
                    {
                        entry.Adjectives.Add(adjective);
                    }
                }
                foreach(var item in request.RelatedTo.Split(","))
                {
                    var relatedTo = await _context.Set<Entry>().FirstOrDefaultAsync(x => x.Name == item);
                    if (relatedTo != null)
                    {
                        entry.RelatedTo.Add(relatedTo);
                    }
                }
                await _context.Set<Entry>().AddAsync(entry);
                var result = await _context.SaveChangesAsync();

                return result;
            }
        }
    }
}
