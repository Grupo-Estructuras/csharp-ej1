namespace csharp_ej1
{
    class Githubscraper
    {
        public static void scrapeGithub(List<string> languages)
        {       
            // DONE Reading from languages string and getting alias from json
            using (StreamReader file = File.OpenText("data/langAliases.json"))
            using (Newtonsoft.Json.JsonTextReader reader = new Newtonsoft.Json.JsonTextReader(file))
            {
                Newtonsoft.Json.Linq.JObject aliasData = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.Linq.JToken.ReadFrom(reader);
                Newtonsoft.Json.Linq.JToken alias;
                
                foreach (var language in languages)
                {
                    try
                    {
                        // WHYYYYYY warning?!?!
                        alias = aliasData.GetValue(language.ToLower()).ToString();
                    }
                    catch (System.NullReferenceException)
                    {
                        alias = language;
                    }

                    // DOING Webscraping github with "alias"


                    Console.WriteLine(alias.ToString());
                }
            }


            // DONE Creating and adding values (just change to the correct format)
            // Creates a file with language and repo ammount
            var filePath = "data/Resultados.txt";

            if (File.Exists(filePath))    
            {    
                File.Delete(filePath);    
            }    
            
            using (StreamWriter fileStr = File.CreateText(filePath))     
            {    
                List<string> myarr = new List<string>();
                myarr.Add("perl");
                myarr.Add("python");
                myarr.Add("kotlin");

                foreach (var item in myarr)
                {
                    fileStr.WriteLine($"{item},");
                }
            }               
        }
    }
}