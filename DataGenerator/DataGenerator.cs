using System;
using Model;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
namespace DataGenerator
{
    class DataGenerator
    {
        static void Main(string[] args)
        {
            Service service = new Service();
            while(true)
            {
                Console.WriteLine(@"Available commands:
                1. Generate garments
                2. Generate orders and order items
                3. Generate clients
                4. Generate reviews
                5. Exit");
                string command = Console.ReadLine();
                string generatorPath = "./../data/generator/";
                switch(command)
                {
                    case "1":
                        ProcessGenerateGarments(generatorPath, service.garmentRepository);
                        break;
                    case "2":
                        ProcessGenerateOrders(service);
                        break;
                    case "3":
                        ProcessGenerateClients(generatorPath, service.clientRepository);
                        break;
                    case "4":
                        ProcessGenerateReviews(service);
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
        static int GetNumberOfEntities(string entity)
        {
            int amount = 0;
            while(true)
            {
                Console.Write($"Enter number of {entity}s you want to generate: ");
                if(int.TryParse(Console.ReadLine(), out amount))
                {
                    if(amount <= 0)
                    {
                        Console.WriteLine($"Error: number must be positive integer. Try again.");
                    }
                    else
                    {
                        return amount;
                    }
                }
                else
                {
                    Console.WriteLine($"Error: number must be integer. Try again.");
                }
            }
        }
        static void ProcessGenerateReviews(Service service)
        {
            int numberOfClients = service.clientRepository.GetCount();
            if(numberOfClients == 0)
            {
                Console.WriteLine("Error: There are no clients in the database. Please, generate them first.");
                return;
            }
            int numberOfGarments = service.garmentRepository.GetCount();
            if(numberOfGarments == 0)
            {
                Console.WriteLine("Error: There are no garments in the database. Please, generate them first.");
                return;
            }
            int numberOfEntities = GetNumberOfEntities("review");
            Console.WriteLine("Data is generating...");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            service.reviewRepository.Generate(numberOfEntities);
            sw.Stop();
            Console.WriteLine($"Generated for [{sw.Elapsed.Milliseconds}] ms!");
        }
        static void ProcessGenerateOrders(Service service)
        {
            int numberOfClients = service.clientRepository.GetCount();
            if(numberOfClients == 0)
            {
                Console.WriteLine("Error: There are no clients in the database. Please, generate them first.");
                return;
            }
            int numberOfGarments = service.garmentRepository.GetCount();
            if(numberOfGarments == 0)
            {
                Console.WriteLine("Error: There are no garments in the database. Please, generate them first.");
                return;
            }
            int numberOfEntities = GetNumberOfEntities("order");
            Console.WriteLine("Data is generating...");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            service.orderRepository.Generate(numberOfEntities);
            sw.Stop();
            Console.WriteLine($"Generated for [{sw.Elapsed.Milliseconds}] ms!");
        }
        static void ProcessGenerateGarments(string dataPath, GarmentRepository repo)
        {
            int numberOfEntities = GetNumberOfEntities("garment");
            string[] names = File.ReadAllText(dataPath + "garment_names.txt").Split("\n");
            string[] brands = File.ReadAllText(dataPath + "brands.txt").Split("\n");
            string[] countries = File.ReadAllText(dataPath + "countries.txt").Split("\n");
            List<Garment> generated = new List<Garment>();
            Random ran = new Random();
            Console.WriteLine("Data is generating...");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for(int i = 0; i<numberOfEntities; i++)
            {
                Garment current = new Garment();
                current.name = names[ran.Next(0, names.Length)];
                current.brand = brands[ran.Next(0, brands.Length)];
                current.cost = ran.Next(20, 1001);
                current.manufacture_country = countries[ran.Next(0, countries.Length)];
                generated.Add(current);
            }
            repo.InsertRange(generated);
            sw.Stop();
            Console.WriteLine($"Generated for [{sw.Elapsed.Milliseconds}] ms!");
        }
        static void ProcessGenerateClients(string dataPath, ClientRepository repo)
        {
            int numberOfEntities = GetNumberOfEntities("client");
            string[] fullnames = File.ReadAllText(dataPath + "fullnames.txt").Split("\n");
            string[] emails = File.ReadAllText(dataPath + "emails.txt").Split("\n");
            List<Client> generated = new List<Client>();
            Random ran = new Random();
            DateTime start = new DateTime(1940,1,1);
            DateTime end = new DateTime(2003,1,1);
            TimeSpan range = end-start;
            Console.WriteLine("Data is generating...");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for(int i = 0; i<numberOfEntities; i++)
            {
                Client current = new Client();
                current.fullname = fullnames[ran.Next(0, fullnames.Length)];
                current.email = emails[ran.Next(0, emails.Length)];
                TimeSpan ts = new TimeSpan((long)(ran.NextDouble() * range.Ticks));
                current.birthday_date = start + ts;
                generated.Add(current);
            }
            repo.InsertRange(generated);
            sw.Stop();
            Console.WriteLine($"Generated for [{sw.Elapsed.Milliseconds}] ms!");
        }
    }
}
