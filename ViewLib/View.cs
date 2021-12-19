using System;
using System.Collections.Generic;
using VisualizationLib;
namespace ViewLib
{
    public class View
    {
        public static void OutputError(string message)
        {
            Console.WriteLine("\n____Error____");
            Console.WriteLine(message);
            Console.WriteLine("_____________\n");
        }
        public static void OutputInsertResult(string entity, int id)
        {
            Console.WriteLine("\n____Inserting____");
            Console.WriteLine($"{entity} was added to the database with id [{id}].\n");
        }
        public static void OutputDeleteResult(string entity, int id)
        {
            Console.WriteLine("\n____Deleting____");
            Console.WriteLine($"{entity} with id [{id}] was deleted sucessfully.\n");
        }
        public static void OutputUpdateResult(string entity, int id)
        {
            Console.WriteLine("\n____Updating____");
            Console.WriteLine($"{entity} info with id [{id}] was updated successfully.\n");
        }
        public static void OutputEntity<T>(T entity)
        {
            Console.WriteLine($"\n____{typeof(T).Name}____");
            Console.WriteLine(entity.ToString());
            Console.WriteLine();
        }
        public static void OutputEntities<T>(List<T> entities)
        {
            Console.WriteLine($"\n____{typeof(T).Name}s____");
            if(entities.Count == 0)
            {
                Console.WriteLine($"There are any {typeof(T).Name}s were found.\n");
                return;
            }
            Console.WriteLine($"There are {entities.Count} were found:\n");
            foreach(T entity in entities)
            {
                Console.WriteLine(entity.ToString());
                Console.WriteLine();
            }
        }
        public static void OutputGarmentRatingChart(List<int> rating, string garmentName)
        {
            Visualization.CreateFootwearRatingsChart(rating, garmentName);
        }
    }
}
