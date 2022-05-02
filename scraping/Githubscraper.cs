using System.Text.RegularExpressions;

namespace csharp_ej1
{
    class Githubscraper
    {
        public static List<Language> scrapeGithub(List<string> languages)
        {       
            // DONE Reading from languages string and getting alias from json
            int min = 0;
            int max = 0;
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

                    // DONE Webscraping github with "alias"
                    var repoAmmount = getRepoAmmount(alias.ToString());

                    min = (min < repoAmmount && min != 0) ? min : repoAmmount;
                    max = (max > repoAmmount) ? max : repoAmmount;

                    Language langObj = new Language(language, repoAmmount, 0.0);
                    langObjArr.Add(langObj);

                    Console.WriteLine($"Scraping...{language}");
                }
            }
            generateFile(langObjArr);

            return updateRatingSorted(langObjArr, min, max);
        }

        private static List<Language> updateRatingSorted(List<Language> langObjArr, int min, int max)
        {
            List<Language> tempLangArr = new List<Language>();

            foreach (var item in langObjArr)
            {
                var newRating = Math.Round((double)(item.getRepoAmmount() - min) / (max - min) * 100, 3);
                tempLangArr.Add(new Language(item.getName(), item.getRepoAmmount(), newRating));
            }

            return tempLangArr;
        }

        // DONE Gets repo ammount from github
        private static int getRepoAmmount(string langAlias)
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