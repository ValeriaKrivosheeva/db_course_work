using System;
using System.Collections.Generic;
namespace ControllerLib
{
    public class GUI
    {
        public static void ProcessUserInterface(Controller controller)
        {
            Console.WriteLine("Hi!");
            while(true)
            {
                Console.WriteLine(@"Select the entity to work with (by numbers):
                1. Garment
                2. Order
                3. Review
                4. Client
                5. Exit");
                string command = Console.ReadLine();
                switch(command)
                {
                    case "1":
                        ProcessGarment(controller);
                        break;
                    case "2":
                        ProcessOrder(controller);
                        break;
                    case "3":
                        ProcessReview(controller);
                        break;
                    case "4":
                        ProcessClient(controller);
                        break;
                    case "5":
                        Console.WriteLine("End.");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Error: Unavailable command. Try again.");
                        break;
                }
            }
        }
        static void ProcessGarment(Controller controller)
        {
            while(true)
            {
                Console.WriteLine(@"Available options for garments (by numbers):
                1. Insert new garment
                2. Update existing garment
                3. Delete existing garment
                4. Return to main menu");
                string command = Console.ReadLine();
                switch(command)
                {
                    case "1":
                        ProcessInsertGarment(controller);
                        break;
                    case "2":
                        ProcessUpdateGarment(controller);
                        break;
                    case "3":
                        controller.DeleteGarment(GetIdToProcess("garment", "delete"));
                        break;
                    case "4":
                        Console.WriteLine("Returning to main menu...");
                        return;
                    default:
                        Console.WriteLine("Error: Unavailable command. Try again.");
                        break;
                }
            }
        }
        static void ProcessInsertGarment(Controller controller)
        {
            Console.WriteLine("Enter new garment info:");
            Console.Write("Name: ");
            string name = Console.ReadLine();
            Console.Write("Brand: ");
            string brand = Console.ReadLine();
            int cost = 0;
            while(true)
            {
                Console.Write("Cost: ");
                if(int.TryParse(Console.ReadLine(), out cost) || cost <= 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Error: cost must be positive integer. Try again.");
                }
            }
            Console.Write("Manufacture country: ");
            string manCountry = Console.ReadLine();
            controller.InsertGarment(name, brand, cost, manCountry);
        }
        static void ProcessUpdateGarment(Controller controller)
        {
            int garmentId = GetIdToProcess("garment", "update");
            Console.WriteLine("Enter updated garment info:");
            Console.Write("Name: ");
            string name = Console.ReadLine();
            Console.Write("Brand: ");
            string brand = Console.ReadLine();
            int cost = 0;
            while(true)
            {
                Console.Write("Cost: ");
                if(int.TryParse(Console.ReadLine(), out cost) || cost <= 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Error: cost must be positive integer. Try again.");
                }
            }
            Console.Write("Manufacture country: ");
            string manCountry = Console.ReadLine();
            controller.UpdateGarment(garmentId, name, brand, cost, manCountry);
        }
        static void ProcessOrder(Controller controller)
        {
            while(true)
            {
                Console.WriteLine(@"Available options for orders (by numbers):
                1. Insert new order
                2. Delete existing order
                3. Return to main menu");
                string command = Console.ReadLine();
                switch(command)
                {
                    case "1":
                        ProcessInsertOrder(controller);
                        break;
                    case "2":
                        controller.DeleteOrder(GetIdToProcess("order", "delete"));
                        break;
                    case "3":
                        Console.WriteLine("Returning to main menu...");
                        return;
                    default:
                        Console.WriteLine("Error: Unavailable command. Try again.");
                        break;
                }
            }
        }
        static void ProcessInsertOrder(Controller controller)
        {
            Console.WriteLine("Enter new order info:");
            int clientId = GetIdToProcess("client", "insert order");
            Console.Write("Shipping method: ");
            string shipMethod = Console.ReadLine();
            List<int> orderItems = new List<int>();
            Console.WriteLine("Add garments to your order by id:");
            while(true)
            {
                Console.WriteLine(@"Select by number:
                1. Add garment
                2. Confirm order");
                string command = Console.ReadLine();
                if(command == "1")
                {
                    orderItems.Add(GetIdToProcess("garment", "add to order"));
                }
                else if(command == "2")
                {
                    if(orderItems.Count == 0)
                    {
                        Console.WriteLine("Error: you have to order at least 1 garment.");
                        continue;
                    }
                    break;
                }
                else
                {
                    Console.WriteLine("Error: Unavailable option. Try again.");
                }
            }
            controller.InsertOrder(shipMethod, clientId, orderItems);
        }
        static void ProcessReview(Controller controller)
        {

        }
        static int GetIdToProcess(string entity, string process)
        {
            int id = 0;
            while(true)
            {
                Console.Write($"Enter {entity} id to {process}: ");
                if(int.TryParse(Console.ReadLine(), out id))
                {
                    break;
                }
                else
                {
                    Console.WriteLine($"Error: {entity} id must be integer. Try again.");
                }
            }
            return id;
        }
        static void ProcessClient(Controller controller)
        {
            while(true)
            {
                Console.WriteLine(@"Available options for clients (by numbers):
                1. Insert new client
                2. Update existing client
                3. Delete existing client
                4. Return to main menu");
                string command = Console.ReadLine();
                switch(command)
                {
                    case "1":
                        ProcessInsertClient(controller);
                        break;
                    case "2":
                        ProcessUpdateClient(controller);
                        break;
                    case "3":
                        controller.DeleteClient(GetIdToProcess("client", "delete"));
                        break;
                    case "4":
                        Console.WriteLine("Returning to main menu...");
                        return;
                    default:
                        Console.WriteLine("Error: Unavailable command. Try again.");
                        break;
                }
            }
        }
        static void ProcessInsertClient(Controller controller)
        {
            Console.WriteLine("Enter new client info:");
            Console.Write("Fullname: ");
            string fullname = Console.ReadLine();
            Console.Write("Email: ");
            string email = Console.ReadLine();
            DateTime birthDate;
            while(true)
            {
                Console.Write("Birth date: ");
                if(DateTime.TryParse(Console.ReadLine(), out birthDate) || birthDate > DateTime.Now || birthDate < new DateTime(1900,1,1))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Error: birth date must be in available date format and range. Try again.");
                }
            }
            controller.InsertClient(fullname, email, birthDate);
        }
        static void ProcessUpdateClient(Controller controller)
        {
            int clientId = GetIdToProcess("client", "update");
            Console.WriteLine("Enter updated client info:");
            Console.Write("Fullname: ");
            string fullname = Console.ReadLine();
            Console.Write("Email: ");
            string email = Console.ReadLine();
            DateTime birthDate;
            while(true)
            {
                Console.Write("Birth date: ");
                if(DateTime.TryParse(Console.ReadLine(), out birthDate) || birthDate > DateTime.Now || birthDate < new DateTime(1900,1,1))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Error: birth date must be in available date format and range. Try again.");
                }
            }
            controller.UpdateClient(clientId, fullname, email, birthDate);
        }
        static void ProcessDeleteClient(Controller controller)
        {

        }
    }
}