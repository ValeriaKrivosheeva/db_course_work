using System;

using System.Collections.Generic;
using ScottPlot;


namespace VisualizationLib
{
    public class ChartsGenerator
    {
        public static void CreateGarmentRatingsChart(List<int> ratings, string gm_name, string filePath)
        {
            var plt = new ScottPlot.Plot(600, 500);
            int pointCount = 10;
            Random rand = new Random();
            double[] xs = DataGen.Consecutive(pointCount);
            double[] ys = MakeYAsixs(ratings, pointCount);
            plt.PlotBar(xs, ys,horizontal: true);
            plt.Grid(lineStyle: LineStyle.Dot);
            string[] labels = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten"};
            plt.XTicks(ForXTicks(ratings));
            plt.YTicks(xs, labels);
            plt.Title(gm_name);
            plt.SaveFig(filePath);

        }
        public static void CreateGarmentClientsChart(List<double> numbersOfClients, string gm_name)
        {
            var plt = new ScottPlot.Plot(600, 500);
            int nulls = 0;
            foreach(double d in numbersOfClients)
            {
                if(d == 0)
                {
                    nulls++;
                }
            }
            if(nulls == numbersOfClients.Count)
            {
                throw new Exception("There are no clients for entered garment.");
            }
            double[] values = numbersOfClients.ToArray();
            var pie = plt.AddPie(values);
            pie.ShowValues = true;
            pie.Explode = true;
            string[] labels = { "0-18", "19-25", "26-35", "36-45", "46+" };
            pie.SliceLabels = labels;
            plt.Legend();
            plt.Title(gm_name);
            plt.SaveFig($"./../data/clientsAge {gm_name}.png");
        }
        public static void CreateHashChart(double[] withoutIndex, double[] withIndex)
        {
            var plt = new ScottPlot.Plot(600, 400);
            double[] values1 = withoutIndex;
            double[] values2 = withIndex;

            double[][] valuesBySeries = { values1, values2 };
            string[] groupNames = { "search clients by fullname", "search garments by brand" };
            string[] seriesNames = { "Without index", "With index" };
        
            plt.AddBarGroups(groupNames, seriesNames, valuesBySeries, null);
            plt.Legend(location: Alignment.UpperRight);
            plt.Title("Hash index");
            plt.XLabel("Hash using");
            plt.YLabel("Milliseconds");

            plt.SetAxisLimits(yMin: 0);
            plt.SaveFig($"./../data/Hash chart.png");
        }
        public static void CreateBtreeChart(double[] withoutIndex, double[] withIndex)
        {
            var plt = new ScottPlot.Plot(600, 400);
            double[] values1 = withoutIndex;
            double[] values2 = withIndex;

            double[][] valuesBySeries = { values1, values2 };
            string[] groupNames = { "search by cost range", "search by rating and posted time" };
            string[] seriesNames = { "Without index", "With index" };
        
            plt.AddBarGroups(groupNames, seriesNames, valuesBySeries, null);
            plt.Legend(location: Alignment.UpperRight);
            plt.Title("Btree index");
            plt.XLabel("Btree using");
            plt.YLabel("Milliseconds");

            plt.SetAxisLimits(yMin: 0);
            plt.SaveFig($"./../data/Btree chart.png");
        }
        public static void CreateIncomesChart(List<double> incomes, int year)
        {
            var plt = new ScottPlot.Plot(1000, 400);

            double[] values = incomes.ToArray();
            double[] xs = DataGen.Consecutive(12);
            string[] labels = new string[]{"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"};
            plt.AddScatter(xs, values);
            plt.XAxis.ManualTickPositions(null, labels);
            plt.XTicks(xs, labels);
            plt.Title($"Incomes in {year}");
            plt.XLabel("Months");
            plt.YLabel("Incomes");

            plt.SaveFig($"./../data/Incomes in {year}.png");
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
            ratings.Sort();
            int max = 0;
            int current = 0;
            int currentMax = 0;
            foreach(int i in ratings)
            {
                if(current != i)
                {
                    current = i;
                    currentMax = 0;
                }
                currentMax++;
                if(currentMax>max)
                {
                    max = currentMax;
                }
            }
            string[] result = new string[max+1];
            for(int i = 0; i<result.Length; i++)
            {
                result[i] = i.ToString();
            }
            return result;
        }
    }
}
