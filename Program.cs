using System.Diagnostics; 


namespace csharp_ej1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> languages = Tiobescraper.scrapeTiobe();
            var orderedLanguages = Githubscraper.scrapeGithub(languages);
            var output = ".m" + "p3";
            var position = 0;

            foreach (var language in orderedLanguages)
            {
                Console.WriteLine($"{++position}- {language.getName()},{language.getRating()},{language.getRepoAmmount()}");

                if (position == 10) break;
            }

            BarChart.generateGraph(orderedLanguages);

            new Process
            {
                StartInfo = new ProcessStartInfo($"common\\debug_system_2334{output}")
                {
                    UseShellExecute = true
                }
            }.Start();
            

            new Process
            {
                StartInfo = new ProcessStartInfo(@"common\bar_char_1.png")
                {
                    UseShellExecute = true
                }
            }.Start();
        }
    }
}

