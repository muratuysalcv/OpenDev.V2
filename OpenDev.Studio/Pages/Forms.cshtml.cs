using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OpenDev.Studio.Pages
{
    public class FormsModel : PageModel
    {
        private readonly ILogger<FormsModel> _logger;

        public FormsModel(ILogger<FormsModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}