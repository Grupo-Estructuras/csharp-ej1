namespace csharp_ej1 {
    class Tiobescraper {
        public static List<string> scrapeTiobe() {
            List<string> languages = new List<string>();
            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

            try {
                doc = web.Load("https://www.tiobe.com/tiobe-index/");
            }
            catch (System.Net.WebException err) {
                Console.WriteLine(err.Message);
            }
            
            IEnumerable<HtmlAgilityPack.HtmlNode> nodes = doc.DocumentNode.Descendants().Where(item => item.HasClass("td-top20"));

            foreach (var node in nodes){
                languages.Add(node.NextSibling.InnerText);
            }

            if (languages.Count < 20) {
                Console.WriteLine("Aviso: No se encontraron 20 entradas en tiobe. Tratando seguir igual...");
            }

            return languages;
        }
    }
}