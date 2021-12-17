using System;

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
    }
}
