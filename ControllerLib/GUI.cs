using System;
using System.Collections.Generic;
namespace ControllerLib
{
    public class GUI
    {
        public static void ProcessUserInterface(Controller controller)
        {
            Console.WriteLine("Hi!\n");
            while(true)
            {
                Console.WriteLine(@"Select the entity to work with (by numbers):
                1. Garment
                2. Order
                3. Review
                4. Client

Select the statistics to get in chart or report format:
                5. Ratings of certain garment
                6. Age category of clients for certain garment
                7. Shop income by months in certain year
                8. Generate certain garment report

Additional options:
                9. Get hash index effectivness chart
                10. Get btree index effectivness chart

                11. Backup
                12. Restore

                13. Exit");
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
                        ProcessRatingChart(controller);
                        break;
                    case "6":
                        ProcessClientsAgeChart(controller);
                        break;
                    case "7":
                        ProcessIncomeChart(controller);
                        break;
                    case "8":
                        controller.GenerateGarmentReport(GetIdToProcess("garment", "generate report"));
                        break;
                    case "9":
                        controller.CreateHashChart();
                        break;
                    case "10":
                        controller.CreateBtreeChart();
                        break;
                    case "11":
                        controller.Backup();
                        break;
                    case "12":
                        controller.Restore("/home/valeria/Desktop/db_course_work/data/backup/12.21.2021.sql");
                        break;
                    case "13":
                        Console.WriteLine("End.");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Error: Unavailable command. Try again.");
                        break;
                }
            }
        }
        static void ProcessRestore()
        {
            Console.Write("Enter file path to restore from: ");
            string filePath = Console.ReadLine();
            
        }
        static void ProcessIncomeChart(Controller controller)
        {
            int year = 0;
            while(true)
            {
                Console.Write("Enter year to view income by months: ");
                if(int.TryParse(Console.ReadLine(), out year))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Error: year must be integer. Try again.");
                }
            }
            controller.CreateIncomeByMonthsChart(year);
        }
        static void ProcessRatingChart(Controller controller)
        {
            int garmentId = GetIdToProcess("garment", "build rating chart");
            controller.CreateGarmentRatingChart(garmentId);
        }
        static void ProcessClientsAgeChart(Controller controller)
        {
            int garmentId = GetIdToProcess("garment", "build clients age chart");
            controller.CreateGarmentClientsAgeChart(garmentId);
        }
        static void ProcessGarment(Controller controller)
        {
            while(true)
            {
                Console.WriteLine(@"Available options for garments (by numbers):
                1. Insert new garment
                2. Update existing garment
                3. Delete existing garment
                4. Get garment by id
                5. Get garments by brand
                6. Get garments by brand and cost range
                7. Return to main menu");
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
                        controller.GetGarmentById(GetIdToProcess("garment", "find"));
                        break;
                    case "5":
                        ProcessGetGarmentsByBrand(controller);
                        break;
                    case "6":
                        ProcessGetGarmentsByCostRange(controller);
                        break;
                    case "7":
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
        static void ProcessGetGarmentsByBrand(Controller controller)
        {
            Console.Write("Enter brand to search by: ");
            string brand = Console.ReadLine();
            controller.GetGarmentsByBrand(brand);
        }
        static void ProcessGetGarmentsByCostRange(Controller controller)
        {
            Console.WriteLine("Enter info to search by:");
            Console.Write("Brand: ");
            string brand = Console.ReadLine();
            int minCost = 0;
            while(true)
            {
                Console.Write("Min cost: ");
                if(int.TryParse(Console.ReadLine(), out minCost) || minCost <= 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Error: min cost must be positive integer. Try again.");
                }
            }
            int maxCost = 0;
            while(true)
            {
                Console.Write("Max cost: ");
                if(int.TryParse(Console.ReadLine(), out maxCost) || maxCost <= 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Error: max cost must be positive integer. Try again.");
                }
            }
            controller.GetGarmentByCostRange(brand, minCost, maxCost);
        }
        static void ProcessOrder(Controller controller)
        {
            while(true)
            {
                Console.WriteLine(@"Available options for orders (by numbers):
                1. Insert new order
                2. Delete existing order
                3. Get order by id
                4. Get orders by client id
                5. Return to main menu");
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
                        controller.GetOrderById(GetIdToProcess("order", "find"));
                        return;
                    case "4":
                        controller.GetOrdersByClientId(GetIdToProcess("client", "search by"));
                        return;
                    case "5":
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
            while(true)
            {
                Console.WriteLine(@"Available options for reviews (by numbers):
                1. Insert new review
                2. Update existing review
                3. Delete existing review
                4. Get reviews by client id
                5. Get reviews by garment id
                6. Return to main menu");
                string command = Console.ReadLine();
                switch(command)
                {
                    case "1":
                        ProcessInsertReview(controller);
                        break;
                    case "2":
                        ProcessUpdateReview(controller);
                        break;
                    case "3":
                        controller.DeleteReview(GetIdToProcess("review", "delete"));
                        break;
                    case "4":
                        controller.GetReviewsByClientId(GetIdToProcess("client", "search by"));
                        break;
                    case "5":
                        controller.GetReviewsByGarmentId(GetIdToProcess("garment", "search by"));
                        break;
                    case "6":
                        Console.WriteLine("Returning to main menu...");
                        return;
                    default:
                        Console.WriteLine("Error: Unavailable command. Try again.");
                        break;
                }
            }
        }
        static void ProcessInsertReview(Controller controller)
        {
            Console.WriteLine("Enter new review info:");
            int clientId = 0;
            while(true)
            {
                Console.Write("Client id: ");
                if(int.TryParse(Console.ReadLine(), out clientId) || clientId <= 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Error: client id must be positive integer. Try again.");
                }
            }
            int garmentId = 0;
            while(true)
            {
                Console.Write("Garment id: ");
                if(int.TryParse(Console.ReadLine(), out garmentId) || garmentId <= 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Error: garment id must be positive integer. Try again.");
                }
            }
            Console.Write("Opinion: ");
            string opinion = Console.ReadLine();
            int rating = 0;
            while(true)
            {
                Console.Write("Rating: ");
                if(int.TryParse(Console.ReadLine(), out rating) || rating <= 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Error: rating must be positive integer. Try again.");
                }
            }
            controller.InsertReview(opinion, rating, clientId, garmentId);
        }
        static void ProcessUpdateReview(Controller controller)
        {
            int reviewId = GetIdToProcess("review", "update");
            Console.WriteLine("Enter updated review info:");
            Console.Write("Opinion: ");
            string opinion = Console.ReadLine();
            int rating = 0;
            while(true)
            {
                Console.Write("Rating: ");
                if(int.TryParse(Console.ReadLine(), out rating) || rating <= 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Error: rating must be positive integer. Try again.");
                }
            }
            controller.UpdateReview(reviewId, opinion, rating);
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
                4. Get client by id
                5. Get clients by fullname
                6. Return to main menu");
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
                        controller.GetClientById(GetIdToProcess("client", "find"));
                        break;
                    case "5":
                        ProcessGetClientsByFullname(controller);
                        break;
                    case "6":
                        Console.WriteLine("Returning to main menu...");
                        return;
                    default:
                        Console.WriteLine("Error: Unavailable command. Try again.");
                        break;
                }
            }
        }
        static void ProcessGetClientsByFullname(Controller controller)
        {
            Console.Write("Enter clients fullname to search by: ");
            string fullname = Console.ReadLine();
            controller.GetClientsByFullname(fullname);
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
    }
}