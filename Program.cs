namespace csharp_ej1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> languages = Tiobescraper.scrapeTiobe();
            
            foreach (var language in languages)
            {
                Console.WriteLine(language);
            }

            Githubscraper.scrapeGithub(languages);
        }
    }
}

