using System;

using System.Collections.Generic;
using ScottPlot;


namespace VisualizationLib
{
    public  class Visualization
    {
        public static void CreateFootwearRatingsChart(List<int> ratings, string fw_name)
        {
            var plt = new ScottPlot.Plot(600, 400);
            int pointCount = 10;
            Random rand = new Random();
            double[] xs = DataGen.Consecutive(pointCount);
            double[] ys = MakeYAsixs(ratings, pointCount);
            plt.PlotBar(xs, ys,horizontal: true);
            plt.Grid(lineStyle: LineStyle.Dot);
            string[] labels = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten"};
            plt.XTicks(ForXTicks(ratings));
            plt.YTicks(xs, labels);
            plt.SaveFig(@"test.png");

        }
        private static double[] MakeYAsixs(List<int> ratings, int count)
        {
            double[] newYs = new double[count];
            for(int i = 0; i < ratings.Count; i++)
            {
                int rating = ratings[i];
                newYs[rating-1] +=1;
            }
            return newYs;
        }
        private static string[] ForXTicks(List<int> ratings)
        {
            int max = 0;
            foreach(int i in ratings)
            {
                if(i>max)
                {
                    max = i;
                }
            }
            string[] result = new string[max];
            for(int i = 0; i<result.Length; i++)
            {
                result[i] = (i).ToString();
            }
            return result;
        }
    }
}
