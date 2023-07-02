using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SmartQuery.Web.Pages.Entries
{
    public class LinkerModel : PageModel
    {
        [FromQuery]
        public int TargetEntryId { get; set; }
        [BindProperty(SupportsGet = true)]
        public Command Data { get; set; } = new Command();
        public void OnGet()
        {
        }

        public class Command : IRequest<int>
        {
            public int TargetEntryId { get; set; }
            public string LinkingEntries { get; set; }
        }


    }
}
