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
            public string Description { get; init; } = "";
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
                if (request.Adjectives != null && request.Adjectives.Contains(',')) {
                    foreach (var adjectiveId in request.Adjectives.TrimEnd(',').Split(","))
                    {
                        Adjective? adjective = _context.Set<Adjective>().FirstOrDefault(x => x.Id == Int32.Parse(adjectiveId));
                        if(adjective != null)
                        {
                            entry.Adjectives.Add(adjective);
                        } 
                        else
                        {
                            continue;
                        }
                    }
                }
                
                //save the entry with adjectives
                await _context.Set<Entry>().AddAsync(entry);
                var result = await _context.SaveChangesAsync();

                if(result > 0)
                {

                    // link entries //
                    if (request.RelatedTo != null && request.RelatedTo.Contains(','))
                    {
                        foreach (var relatedEntryId in request.RelatedTo.TrimEnd(',').Split(","))
                        {
                            Entry? relatedEntry = _context.Set<Entry>().FirstOrDefault(x => x.Id == Int32.Parse(relatedEntryId));
                            if (relatedEntry != null)
                            {
                                _context.Set<EntryEntry>().AddRange(new List<EntryEntry>() {
                                new EntryEntry()
                            {
                                EntryId = entry.Id,
                                RelatedEntryId = relatedEntry.Id
                            },  new EntryEntry()
                            {
                                RelatedEntryId = entry.Id,
                                EntryId = relatedEntry.Id
                            }
                            });

                                result = await _context.SaveChangesAsync();
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                }

                // if everything went ok (all savechanges) the return will be true;
                return result;
            }
        }
    }
}
