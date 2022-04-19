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
                "nytimes" => ScrapeNewYorkTimes(htmlContent),
                "theglobeandmail" => ScrapeGlobeAndMail(htmlContent),
                _ => new List<string> { $"ERROR - Not a supported paper. Current supported papers are: Economist, NYT" },
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

        public List<string> ScrapeNewYorkTimes(string htmlContent)
        {
            var splitByPara = htmlContent.Split("<p");
            var partsWithParaTag = splitByPara.Where(para => para.Contains("css-g5piaz evys1bk0") && para.Length < 10000);
            var refinedParagraphs = partsWithParaTag.Select(para => para.GetStringBetween(">", "</p", false, false).StripHTML()).ToList();

            var title = htmlContent.GetStringBetween("<title", "</title>", true, false).GetStringAfter(">");
            refinedParagraphs.Insert(0, title);

            return refinedParagraphs;
        }

        public List<string> ScrapeGlobeAndMail(string htmlContent)
        {
            var splitByPara = htmlContent.Split("<p");
            var partsWithParaTag = splitByPara.Where(para => para.Contains("c-article-body__text") && para.Length < 10000);
            var refinedParagraphs = partsWithParaTag.Select(para => para.GetStringBetween(">", "</p", false, false).StripHTML()).ToList();

            var title = htmlContent.GetStringBetween("<title", "</title>", true, false).GetStringAfter(">");
            refinedParagraphs.Insert(0, title);

            return refinedParagraphs;
        }
    }
}
