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
            if (result != null)
            {
                Data = new Command();
                ModelState.Clear();
            }
            return RedirectToPage("/Entries/Linker",new { TargetId=result.Id});

        }
        public record Command : IRequest<Entry>
        {
            public string Name { get; init; }
            public string Description { get; init; } = "";
            public string Adjectives { get; init; }
            public string RelatedTo { get; init; }
        }
        public class CommandHandler : IRequestHandler<Command, Entry>
        {
            private readonly SmartQueryDbContext _context;

            public CommandHandler(SmartQueryDbContext context)
            {
                _context = context;
            }

            public async Task<Entry> Handle(Command request, CancellationToken cancellationToken)
            {
                Entry entry = new Entry()
                {
                    Name = request.Name,
                    Description = request.Description,
                    Slug = new Slugify.SlugHelper().GenerateSlug(request.Name),
                    CreatedAt = DateTimeOffset.UtcNow
                };
                if (request.Adjectives != null)
                {

                    if (request.Adjectives.Contains(','))
                    {
                        foreach (var adjectiveId in request.Adjectives.TrimEnd(',').Split(","))
                        {
                            Adjective? adjective = _context.Set<Adjective>().FirstOrDefault(x => x.Id == Int32.Parse(adjectiveId));
                            if (adjective != null)
                            {
                                entry.Adjectives.Add(adjective);
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    else if (!String.IsNullOrEmpty(request.Adjectives))
                    {
                        Adjective? adjective = _context.Set<Adjective>().FirstOrDefault(x => x.Id == Int32.Parse(request.Adjectives));
                        if (adjective != null)
                        {
                            entry.Adjectives.Add(adjective);
                        }

                    }
                }

                await _context.Set<Entry>().AddAsync(entry);
                var result = await _context.SaveChangesAsync();

                return entry;
            }
        }
    }
}
