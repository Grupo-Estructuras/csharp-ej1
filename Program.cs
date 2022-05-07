using System.Diagnostics;


namespace csharp_ej1
{
    class Program
    {
        static void Main(string[] args)
        {
            // List<string> languages = Tiobescraper.scrapeTiobe();
            // var orderedLanguages = Githubscraper.scrapeGithub(languages);
            // var position = 0;

            // foreach (var language in orderedLanguages)
            // {
            //     Console.WriteLine($"{++position}- {language.getName()},{language.getRating()},{language.getRepoAmmount()}");

            //     if (position == 10) break;
            // }

            // BarChart.generateGraph(orderedLanguages);

            // new Process
            // {
            //     StartInfo = new ProcessStartInfo($"common{System.IO.Path.DirectorySeparatorChar}bar_graph.png")
            //     {
            //         UseShellExecute = true
            //     }
            // }.Start();

            Githubscraper2.testMethod("Chad");

        }
    }
}

