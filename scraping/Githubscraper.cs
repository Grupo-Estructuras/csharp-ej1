using System.Text.RegularExpressions;

namespace csharp_ej1
{
    class Githubscraper
    {
        public static void scrapeGithub(List<string> languages)
        {       
            // DONE Reading from languages string and getting alias from json
            List<Language> langObjArr = new List<Language>();
            using (StreamReader file = File.OpenText("data/langAliases.json"))
            using (Newtonsoft.Json.JsonTextReader reader = new Newtonsoft.Json.JsonTextReader(file))
            {
                Newtonsoft.Json.Linq.JObject aliasData = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.Linq.JToken.ReadFrom(reader);
                Newtonsoft.Json.Linq.JToken alias;
                
                foreach (var language in languages)
                {
                    try
                    {
                        // WHYYYYYY warning?!?! 🔥👌
                        alias = aliasData.GetValue(language.ToLower()).ToString();
                    }
                    catch (System.NullReferenceException)
                    {
                        alias = language;
                    }

                    // DOING Webscraping github with "alias"
                    var repoAmmount = getRepoAmmount(alias.ToString());
                    
                    Language langObj = new Language(language, repoAmmount, 0.0);
                    langObjArr.Add(langObj);

                    Console.WriteLine($"Scraping...{language}");
                }
            }
            // Console.WriteLine("langObjArr.First().getName()");
            generateFile(langObjArr); 
        }

     
        // DONE Gets repo ammount from github
        public static int getRepoAmmount(string langAlias)
        {
            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            IEnumerable<HtmlAgilityPack.HtmlNode> nodes;

            try
            {
                doc = web.Load($"https://github.com/topics/{langAlias}");
                nodes = doc.DocumentNode.Descendants().Where(item => item.HasClass("h3"));
            }
            catch (System.Net.WebException err)
            {
                Console.WriteLine(err.Message);

                return 0;
            }

            var foundData = nodes.Last().InnerText;
            var repoAmmount = Regex.Match(foundData, @"\d+(,\d*)*").Value;
            repoAmmount = repoAmmount.Replace(",", "");
            // Console.WriteLine(repoAmmount);
            return Int32.Parse(repoAmmount);
        }

        private static void generateFile(List<Language> langObjArr) 
        {
            // DONE Creating and adding values
            // Creates a file with language and repo ammount
            var filePath = "data/Resultados.txt";

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (StreamWriter fileStr = File.CreateText(filePath))
            {
                foreach (var item in langObjArr)
                {
                    fileStr.WriteLine($"{item.getName()},{item.getRepoAmmount()}");
                }
            }
        }
    }
}