using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace csharp_ej1
{
    class Githubscraper
    {
        public static List<Language> scrapeGithub(List<string> languages)
        {       
            // DONE Reading from languages string and getting alias from json
            int min = int.MaxValue;
            int max = int.MinValue;
            List<Language> langObjArr = new List<Language>();
            
            try 
            {
                using (StreamReader file = File.OpenText("data/langAliases.json"))
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JObject aliasData = (JObject)JToken.ReadFrom(reader);
                    JToken alias;
                    
                    foreach (var language in languages)
                    {   
                        // Se prueba si existe un alias, caso negativo se usa el original
                        if (!aliasData.TryGetValue(language.ToLower(), out var value))
                        {
                            value = (JToken)language;
                        }
                        
                        alias = value.ToString();
                        
                        var repoAmmount = getRepoAmmount(alias.ToString());

                        min = (min < repoAmmount) ? min : repoAmmount;
                        max = (max > repoAmmount) ? max : repoAmmount;

                        Language langObj = new Language(language, repoAmmount, 0.0);
                        langObjArr.Add(langObj);

                        Console.WriteLine($"Scraping...{language}");
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("No posee perimsos para abrir el archivo");
                Environment.Exit(0);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("El archivo no se encuentra, verifique la ruta del archivo");
                Environment.Exit(0);
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("No se encuentra el directorio del archivo, verifique la ruta del directorio");
                Environment.Exit(0);
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

            tempLangArr.Sort((Language item1, Language item2) => item2.getRepoAmmount().CompareTo(item1.getRepoAmmount()));

            return tempLangArr;
        }

        // Gets repo ammount from github
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
            // Creates a file with language and repo ammount
            var filePath = "data/Resultados.txt";
            
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            try
            {
                using (StreamWriter fileStr = File.CreateText(filePath))
                {
                    foreach (var item in langObjArr)
                    {
                        fileStr.WriteLine($"{item.getName()},{item.getRepoAmmount()}");
                    }
                }
            }
            catch (IOException)
            {
                Console.WriteLine("El archivo no puede abrirse, porfavor intente cerrar el archivo");
            }
        }
    }
}