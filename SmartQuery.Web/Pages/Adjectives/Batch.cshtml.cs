using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SmartQuery.Web.Data;
using SmartQuery.Web.Models;

namespace SmartQuery.Web.Pages.Adjectives
{
    public class BatchModel : PageModel
    {
        private readonly IMediator _mediator;

        public BatchModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public RemoveWhiteSpacesFromAllRequest Command { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //return Command.GetType().Name;
            await _mediator.Send(new RemoveWhiteSpacesFromAllRequest());
            return RedirectToPage("/Adjectives/Batch");

        }
        public class RemoveWhiteSpacesFromAllRequest : IRequest<bool> { }
        public class RemoveWhiteSpacesFromAllHandler : IRequestHandler<RemoveWhiteSpacesFromAllRequest, bool>
        {
            private readonly SmartQueryDbContext _context;

            public RemoveWhiteSpacesFromAllHandler(SmartQueryDbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(RemoveWhiteSpacesFromAllRequest request, CancellationToken cancellationToken)
            {
                await _context.Set<Adjective>()
                    .ForEachAsync(x => { x.Slug = x.Slug.Trim();x.Name = x.Name.Trim(); });
                int result= await  _context.SaveChangesAsync();
                if(result > 0) { return true; }
                return false;
            }
        }
        public class FixSlugsAllRequest : IRequest<bool> {}
        public class FixSlugsAllHandler : IRequestHandler<FixSlugsAllRequest,bool> {
            private readonly SmartQueryDbContext _context;

            public FixSlugsAllHandler(SmartQueryDbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(FixSlugsAllRequest request, CancellationToken cancellationToken)
            {
                await _context.Set<Adjective>()
                   .ForEachAsync(x => x.Slug = new Slugify.SlugHelper().GenerateSlug(x.Name));
                int result = await _context.SaveChangesAsync();
                if (result > 0) { return true; }
                return false;
            }
        }
    }
}
