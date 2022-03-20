using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reader.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ArticleReader _articleReader;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            _articleReader = new ArticleReader();
            Article = new List<string> { "Please enter the article URL" };
        }

        public void OnGet()
        {

        }

        [BindProperty]
        public string ArticleUrl { get; set; }
        public void OnPost(string articleUrl)
        {
            Article = _articleReader.ReadArticle(articleUrl);
        }

        public List<string> Article { get; set; }
    }
}
