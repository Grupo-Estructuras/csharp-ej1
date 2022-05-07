using System.Text.RegularExpressions;

namespace csharp_ej1
{
    class Githubscraper2
    {

        
        public static string getTopic(string langAlias)
        {
            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            IEnumerable<HtmlAgilityPack.HtmlNode> nodes;

            try
            {
                doc = web.Load($"https://github.com/topics/{langAlias}?o=desc&s=updated&page=1");
                nodes = doc.DocumentNode.Descendants().Where(item => item.HasClass("color-bg-default"));
            }
            catch (System.Net.WebException err)
            {
                Console.WriteLine(err.Message);

                return "";
            }
// var timeData = Regex.Match(node_data, "datetime=\"[^\"]+").Value;
            // Busca la fecha
            // var foundData = nodes.Last().InnerHtml;
            // var time = Regex.Match(foundData, "datetime=\"[^\"]+").Value;

            foreach (var node in nodes)
            {
                var node_data = node.InnerHtml;
                var timeData = Regex.Match(node_data, "datetime=").Value;
                // var cleanTimeData = Regex.Replace(timeData, @"\s+", String.Empty);
                // Console.WriteLine(node_data);
                var tagListData = node.ChildNodes;
                // Console.WriteLine(cleanTimeData);
                if (String.IsNullOrEmpty(timeData))
                {
                    Console.WriteLine(timeData);
                }
                foreach (var tagData in tagListData)
                {
                    
                    Console.WriteLine("");
                }
                // var currTime = time.ToString();
                // Console.WriteLine(cleanData);
            }

            // nodes = doc.DocumentNode.Descendants().Where(item => item.HasClass("topic-tag"));
            // foreach (var node in nodes)
            // {
            //     var node_data = node.InnerText;
            //     var cleanData = Regex.Replace(node_data, @"\s+", String.Empty);
                
            //     // var time = Regex.Match(node_data, "datetime=\"[^\"]+").Value;
            //     // Thread.Sleep(2000);

            //     // Console.WriteLine(cleanData);
            // }
            return "";
            // Retorna solo el elemento de datetime
            // return time.Substring(10);

        }

        public static void testMethod(string langAlias)
        {
            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            IEnumerable<HtmlAgilityPack.HtmlNode> nodes;

            try
            {
                doc = web.Load($"https://github.com/topics/{langAlias}?o=desc&s=updated&page=1");
                nodes = doc.DocumentNode.Descendants().Where(item => item.HasClass("my-4"));
            }
            catch (System.Net.WebException err)
            {
                Console.WriteLine(err.Message);
                return;
            }
            

            foreach (var node in nodes)
            {
                var node_data = node.InnerHtml;
                var timeData = Regex.Match(node_data, "mr-4[\\S\\s]*datetime=\"[^\"]+").Value;
                timeData = Regex.Match(timeData, "datetime=\"[^\"]+").Value;
                timeData = Regex.Match(timeData, "[^\"]*$").Value;

                // Si encuentra el patron de fecha, se buscan los topics relacionados
                if (!String.IsNullOrEmpty(timeData))
                {
                    Console.WriteLine(timeData);
                    var topicData = node.HasClass("")
                }

            }


        }
    }
}