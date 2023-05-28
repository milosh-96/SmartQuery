using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SmartQuery.Web.Data;
using SmartQuery.Web.Models;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace SmartQuery.Web.Pages.Entries
{
    public class SearchModel : PageModel
    {
        private IMediator _mediator;

        public SearchModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty(SupportsGet = true)]
        public SearchMode Mode { get; set; } = SearchMode.Undefined;

        [BindProperty(SupportsGet = true)]
        public string Query { get; set; } = "";

        public List<Entry> Results { get; set; } = new List<Entry>();

        public async Task OnGetAsync()
        {
            if(Query != null && !String.IsNullOrEmpty(Query))
            {
                var queryValuesList = Query.Split(',').ToList();
                Results = await _mediator.Send(new SearchEntriesInAndMode()
                {
                    Values = queryValuesList
                });
            }
        }

        public class SearchEntriesInAndMode : IRequest<List<Entry>> {
            public List<string> Values { get; set; } = new List<string>();   
        }
        public class SearchEntriesInAndModeHandler : IRequestHandler<SearchEntriesInAndMode, List<Entry>>
        {
            private readonly SmartQueryDbContext _context;

            public SearchEntriesInAndModeHandler(SmartQueryDbContext context)
            {
                _context = context;
            }

            public async Task<List<Entry>> Handle(SearchEntriesInAndMode request, CancellationToken cancellationToken)
            {
                List<Entry> entries = new List<Entry>();
                IQueryable<Entry> query = _context.Set<Entry>().Include(x=>x.RelatedEntries).AsSingleQuery();
                var searchItems = new List<int>();
                foreach (string item in request.Values)
                {
                    var findEntry = _context.Set<Entry>().FirstOrDefault(x => x.Name.ToLower() == item.ToLower());
                    if (findEntry == null) { return entries; }
                    int id = findEntry.Id;
                    searchItems.Add(id);

                   
                }
                //fetch all items that have the first item in the list//
                entries.AddRange(
                    query.Where(
                        x => x.RelatedEntries.Select(
                            x => x.RelatedEntryId).Contains(searchItems[0]))
                    );
                //check other elements one by one//
                for(int i = 1;i < searchItems.Count;i++)
                {
                     entries.RemoveAll(x=> !x.RelatedEntries.Select(
                            x => x.RelatedEntryId).Contains(searchItems[i])
                            );
                }
                //add items that finish all steps//

                return entries;
            }
        }
          public class SearchEntriesInOrMode : IRequest<List<Entry>> {
            public List<string> Values { get; set; } = new List<string>();   
        }
        public class SearchEntriesInOrModeHandler : IRequestHandler<SearchEntriesInOrMode, List<Entry>>
        {
            private readonly SmartQueryDbContext _context;

            public SearchEntriesInOrModeHandler(SmartQueryDbContext context)
            {
                _context = context;
            }

            public async Task<List<Entry>> Handle(SearchEntriesInOrMode request, CancellationToken cancellationToken)
            {
                IQueryable<Entry> query = _context.Set<Entry>().AsSingleQuery();
                foreach(string item in request.Values) {
                    var findEntry = _context.Set<Entry>().FirstOrDefault(x => x.Name.ToLower() == item.ToLower());
                    if(findEntry == null) { continue; }
                    int id = findEntry.Id;

                    //query.Where(x => x.RelatedTo.Contains(findEntry));
                }
                return await query.ToListAsync();
            }
        }

        public enum SearchMode
        {
            Or = 1,
            And = 2,
           Undefined = 0,

        }

    }
}
