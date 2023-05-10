using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Windows.Input;

namespace SmartQuery.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IMediator _mediator;

   


        public string Data { get; set; } = "";
        public IndexModel(ILogger<IndexModel> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task OnGetAsync()
        {
            this.Data = await _mediator.Send(new Command());
        }


         class Command : IRequest<string> { }
         class CommandResponse : IRequestHandler<Command, string>
        {
            public Task<string> Handle(Command request, CancellationToken cancellationToken)
            {
                return Task.FromResult("x1f");
            }
        }
    }
}