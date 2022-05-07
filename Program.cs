using System.Diagnostics;


namespace csharp_ej1
{
    class Program
    {
        static void Main(string[] args)
        {
            var languages = Tiobescraper.scrapeTiobe();
            var orderedLanguages = Githubscraper.scrapeGithub(languages);
            var position = 0;

            foreach (var language in orderedLanguages)
            {
                Console.WriteLine($"{++position}- {language.getName()},{language.getRating()},{language.getRepoAmmount()}");

                if (position == 10) break;
            }

            BarChart.generateGraph(orderedLanguages, 10, "bar_graph.png");

            new Process
            {
                StartInfo = new ProcessStartInfo($"common{System.IO.Path.DirectorySeparatorChar}bar_graph.png")
                {
                    UseShellExecute = true
                }
            }.Start();

            // Parte 2
            var dic = Githubscraper2.getTopics("chad");
            var sortedDic = Githubscraper2.sortDictionary(dic);
            Githubscraper.generateFile(sortedDic, "Resultados2.txt");
            BarChart.generateGraph(sortedDic, 20, "bar_graph2.png");

            new Process
            {
                StartInfo = new ProcessStartInfo($"common{System.IO.Path.DirectorySeparatorChar}bar_graph2.png")
                {
                    UseShellExecute = true
                }
            }.Start();
        }
    }
}
