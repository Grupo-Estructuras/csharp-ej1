

namespace csharp_ej1
{
    class BarChart
    {
        
        public static void generateGraph(List<Language> langArr)
        {
            var plt = new ScottPlot.Plot(1200, 800);

            double[] positions = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
            string[] labels = new string[10];
            double[] values = new double[10];

            for (int i = 0; i < 10; i++)
            {
                labels[i] = (langArr[i].getName());
                values[i] = (langArr[i].getRepoAmmount());
            }

            plt.AddBar(values, positions);
            plt.XTicks(positions, labels);
            plt.SetAxisLimits(yMin: 0);

            plt.SaveFig("bar_graph.png");
        }
    }
}



