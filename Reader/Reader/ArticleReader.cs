using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Reader
{
    public class ArticleReader
    {
        private readonly WebClient _webClient;

        private const char QUOTE = '"';

        public ArticleReader()
        {
            _webClient = new WebClient();
        }

        public List<string> ReadArticle(string articleUrl)
        {
            string htmlContent;
            string newspaper;

            try
            {
                htmlContent = _webClient.DownloadString(articleUrl);
                newspaper = articleUrl.GetStringBetween("www.", ".co", true, false);
            }
            catch
            {
                return new List<string> { "ERROR - Invalid URL" };
            }

            return newspaper switch
            {
                "economist" => ScrapeEconomist(htmlContent),
                _ => new List<string> { $"ERROR - Not a supported paper. Current supported papers are: Economist" },
            };
        }

        public List<string> ScrapeEconomist(string htmlContent)
        {
            var splitByPara = htmlContent.Split("<p");
            var partsWithParaTag = splitByPara.Where(para => para.Contains("article__body-text") && para.Length < 10000);
            var refinedParagraphs = partsWithParaTag.Select(para => para.GetStringBetween(">", "</p", false, false).StripHTML()).ToList();
            
            var title = htmlContent.GetStringBetween("<title>", "</title>", true, false);
            refinedParagraphs.Insert(0, title);

            return refinedParagraphs;
        }
    }
}
