﻿using System;
using Model;
using ViewLib;
using System.Collections.Generic;
using VisualizationLib;
using System.Diagnostics;
using System.IO;

namespace ControllerLib
{
    public class Controller
    {
        private Service service;
        public Controller()
        {
            this.service = new Service();
        }
        public void InsertGarment(string name, string brand, int cost, string manCountry)
        {
            Garment garmentToInsert = new Garment(){name = name, brand = brand, cost = cost, manufacture_country = manCountry};
            try
            {
                int result = service.garmentRepository.Insert(garmentToInsert);
                View.OutputInsertResult(typeof(Garment).Name, result);
            }
            catch(Exception ex)
            {
                View.OutputError(ex.Message);
            }
        }
        public void DeleteGarment(int garmentId)
        {
            Garment garmentById = service.garmentRepository.GetById(garmentId);
            if(garmentById == null)
            {
                View.OutputError($"Garment with id [{garmentId}] doesn`t exist.");
                return;
            }
            try
            {
                service.reviewRepository.DeleteByGarmentId(garmentId);
                service.orderItemRepository.DeleteByGarmentId(garmentId);
                service.garmentRepository.DeleteById(garmentId);
                View.OutputDeleteResult(typeof(Garment).Name, garmentId);
            }
            catch(Exception ex)
            {
                View.OutputError(ex.Message);
            }
        }
        public void UpdateGarment(int garmentId, string name, string brand, int cost, string manCountry)
        {
            Garment garmentById = service.garmentRepository.GetById(garmentId);
            if(garmentById == null)
            {
                View.OutputError($"Garment with id [{garmentId}] doesn`t exist.");
                return;
            }
            Garment garmentToInsert = new Garment(){id = garmentId, name = name, brand = brand, cost = cost, manufacture_country = manCountry};
            try
            {
                service.garmentRepository.Update(garmentId, garmentToInsert);
                View.OutputUpdateResult(typeof(Garment).Name, garmentId);
            }
            catch(Exception ex)
            {
                View.OutputError(ex.Message);
            }
        }
        public void GetGarmentById(int garmentId)
        {
            Garment garmentById = service.garmentRepository.GetById(garmentId);
            if(garmentById == null)
            {
                View.OutputError($"Garment with id [{garmentId}] doesn`t exist.");
                return;
            }
            View.OutputEntity<Garment>(garmentById);
        }
        public void GetGarmentsByBrand(string brand)
        {
            List<Garment> result = service.garmentRepository.GetByBrand(brand);
            View.OutputEntities<Garment>(result);
        }
        public void GetGarmentByCostRange(string brand, int minCost, int maxCost)
        {
            if(minCost > maxCost)
            {
                View.OutputError("Min cost cannot be more than max cost.");
                return;
            }
            List<Garment> garments = service.garmentRepository.GetByCostRangeAndBrand(brand, minCost, maxCost);
            View.OutputEntities<Garment>(garments);
        }
        public void InsertOrder(string shipMethod, int clientId, List<int> garments)
        {
            Client clientById = service.clientRepository.GetById(clientId);
            if(clientById == null)
            {
                View.OutputError($"Client with id [{clientId}] doesn`t exist.");
                return;
            }
            foreach(int garm in garments)
            {
                Garment garmentById = service.garmentRepository.GetById(garm);
                if(garmentById == null)
                {
                    View.OutputError($"Garment with id [{garm}] doesn`t exist.");
                    return;
                }
            }
            Order orderToInsert = new Order(){shipping_method = shipMethod, client_id = clientId, created_date = DateTime.Now};
            try
            {
                int orderId = service.orderRepository.Insert(orderToInsert);
                foreach(int garm in garments)
                {
                    OrderItem orderItem = new OrderItem(){garment_id = garm, order_id = orderId};
                    service.orderItemRepository.Insert(orderItem);
                }
                View.OutputInsertResult(typeof(Order).Name, orderId);
            }
            catch(Exception ex)
            {
                View.OutputError(ex.Message);
            }
        }
        public void DeleteOrder(int orderId)
        {
            Order orderById = service.orderRepository.GetById(orderId);
            if(orderById == null)
            {
                View.OutputError($"Order with id [{orderId}] doesn`t exist.");
                return;
            }
            try
            {
                service.orderItemRepository.DeleteByOrderId(orderId);
                service.orderRepository.DeleteById(orderId);
                View.OutputDeleteResult(typeof(Order).Name, orderId);
            }
            catch(Exception ex)
            {
                View.OutputError(ex.Message);
            }
        }
        public void GetOrderById(int orderId)
        {
            Order orderById = service.orderRepository.GetById(orderId);
            if(orderById == null)
            {
                View.OutputError($"Order with id [{orderId}] doesn`t exist.");
                return;
            }
            List<int> orderItems = service.orderItemRepository.GetByOrderId(orderId);
            List<Garment> garments = new List<Garment>();
            foreach(int item in orderItems)
            {
                garments.Add(service.garmentRepository.GetById(item));
            }
            View.OutputEntity<Order>(orderById);
            View.OutputEntities<Garment>(garments);
        }
        public void GetOrdersByClientId(int clientId)
        {
            Client clientById = service.clientRepository.GetById(clientId);
            if(clientById == null)
            {
                View.OutputError($"Client with id [{clientId}] doesn`t exist.");
                return;
            }
            List<Order> result = service.orderRepository.GetByClientId(clientId);
            View.OutputEntities<Order>(result);
        }
        public void InsertClient(string fullname, string email, DateTime birthDate)
        {
            Client clientToInsert = new Client(){fullname = fullname, email = email, birthday_date = birthDate};
            try
            {
                int result = service.clientRepository.Insert(clientToInsert);
                View.OutputInsertResult(typeof(Client).Name, result);
            }
            catch(Exception ex)
            {
                View.OutputError(ex.Message);
            }
        }
        public void UpdateClient(int clientId, string fullname, string email, DateTime birthDate)
        {
            Client clientById = service.clientRepository.GetById(clientId);
            if(clientById == null)
            {
                View.OutputError($"Client with id [{clientId}] doesn`t exist.");
                return;
            }
            Client clientToUpdate = new Client(){id = clientId, fullname = fullname, email = email, birthday_date = birthDate};
            try
            {
                service.clientRepository.Update(clientId, clientToUpdate);
                View.OutputUpdateResult(typeof(Client).Name, clientId);
            }
            catch(Exception ex)
            {
                View.OutputError(ex.Message);
            }
        }
        public void DeleteClient(int clientId)
        {
            Client clientById = service.clientRepository.GetById(clientId);
            if(clientById == null)
            {
                View.OutputError($"Client with id [{clientId}] doesn`t exist.");
                return;
            }
            try
            {
                service.orderRepository.DeleteByClientId(clientId);
                service.reviewRepository.DeleteByClientId(clientId);
                service.clientRepository.DeleteById(clientId);
                View.OutputDeleteResult(typeof(Client).Name, clientId);
            }
            catch(Exception ex)
            {
                View.OutputError(ex.Message);
            }
        }
        public void GetClientById(int clientId)
        {
            Client clientById = service.clientRepository.GetById(clientId);
            if(clientById == null)
            {
                View.OutputError($"Client with id [{clientId}] doesn`t exist.");
                return;
            }
            View.OutputEntity<Client>(clientById);
        }
        public void GetClientsByFullname(string fullname)
        {
            List<Client> clients = service.clientRepository.GetByFullname(fullname);
            View.OutputEntities<Client>(clients);
        }
        public void InsertReview(string opinion, int rating, int clientId, int garmentId)
        {
            Client clientById = service.clientRepository.GetById(clientId);
            if(clientById == null)
            {
                View.OutputError($"Client with id [{clientId}] doesn`t exist.");
                return;
            }
            Garment garmentById = service.garmentRepository.GetById(garmentId);
            if(garmentById == null)
            {
                View.OutputError($"Garment with id [{garmentId}] doesn`t exist.");
                return;
            }
            Review reviewToInsert = new Review(){opinion = opinion, rating = rating, posted_at = DateTime.Now, client_id = clientId, garment_id = garmentId};
            try
            {
                int result = service.reviewRepository.Insert(reviewToInsert);
                View.OutputInsertResult(typeof(Review).Name, result);
            }
            catch(Exception ex)
            {
                View.OutputError(ex.Message);
            }
        }
        public void UpdateReview(int reviewId, string opinion, int rating)
        {
            Review reviewById = service.reviewRepository.GetById(reviewId);
            if(reviewById == null)
            {
                View.OutputError($"Review with id [{reviewId}] doesn`t exist.");
                return;
            }
            Review reviewToUpdate = new Review(){id = reviewId, opinion = opinion, rating = rating, client_id = reviewById.client_id,
            garment_id = reviewById.garment_id, posted_at = reviewById.posted_at};
            try
            {
                service.reviewRepository.Update(reviewId, reviewToUpdate);
                View.OutputUpdateResult(typeof(Review).Name, reviewId);
            }
            catch(Exception ex)
            {
                View.OutputError(ex.Message);
            }
        }
        public void DeleteReview(int reviewId)
        {
            Review reviewById = service.reviewRepository.GetById(reviewId);
            if(reviewById == null)
            {
                View.OutputError($"Review with id [{reviewId}] doesn`t exist.");
                return;
            }
            try
            {
                service.reviewRepository.DeleteById(reviewId);
                View.OutputDeleteResult(typeof(Review).Name, reviewId);
            }
            catch(Exception ex)
            {
                View.OutputError(ex.Message);
            }
        }
        public void GetReviewsByClientId(int clientId)
        {
            Client clientById = service.clientRepository.GetById(clientId);
            if(clientById == null)
            {
                View.OutputError($"Client with id [{clientId}] doesn`t exist.");
                return;
            }
            List<Review> result = service.reviewRepository.GetByClientId(clientId);
            View.OutputEntities<Review>(result); 
        }
        public void GetReviewsByGarmentId(int garmentId)
        {
            Garment garmentById = service.garmentRepository.GetById(garmentId);
            if(garmentById == null)
            {
                View.OutputError($"Garment with id [{garmentId}] doesn`t exist.");
                return;
            }
            List<Review> result = service.reviewRepository.GetByGarmentId(garmentId);
            View.OutputEntities<Review>(result); 
        }
        public void CreateGarmentRatingChart(int garmentId)
        {
            Garment garmentById = service.garmentRepository.GetById(garmentId);
            if(garmentById == null)
            {
                View.OutputError($"Garment with id [{garmentId}] doesn`t exist.");
                return;
            }
            List<int> ratings = service.reviewRepository.GetGarmentRatings(garmentId);
            View.OutputGarmentRatingChart(ratings, garmentById.name);
        }
        public void CreateGarmentClientsAgeChart(int garmentId)
        {
            Garment garmentById = service.garmentRepository.GetById(garmentId);
            if(garmentById == null)
            {
                View.OutputError($"Garment with id [{garmentId}] doesn`t exist.");
                return;
            }
            List<double> numberOfClients = service.garmentRepository.GetClientsAge(garmentId);
            View.OutputGarmentClientsAgeChart(numberOfClients, garmentById.name);
        }
        public void CreateHashChart()
        {
            int clientsCount = service.clientRepository.GetCount();
            int garmentCount = service.garmentRepository.GetCount();
            if(clientsCount == 0 || garmentCount == 0)
            {
                View.OutputError("There aren`t enough entities to conduct test.");
                return;
            }
            string fullname = service.clientRepository.GetRandomFullnameForChart();
            string brand = service.garmentRepository.GetRandomBrandForChart();
            double[] withoutIndexes = new double[2];
            Stopwatch sw = new Stopwatch();
            service.clientRepository.DropFullnameIndex();
            sw.Start();
            List<Client> clientsById = service.clientRepository.GetByFullname(fullname);
            sw.Stop();
            withoutIndexes[0] = sw.ElapsedMilliseconds;
            sw.Reset();
            service.garmentRepository.DropBrandIndex();
            sw.Start();
            List<Garment> garmentsById = service.garmentRepository.GetByBrand(brand);
            sw.Stop();
            withoutIndexes[1] = sw.ElapsedMilliseconds;
            sw.Reset();

            double[] withIndexes = new double[2];
            service.clientRepository.CreateFullnameIndex();
            sw.Start();
            List<Client> clients = service.clientRepository.GetByFullname(fullname);
            sw.Stop();
            withIndexes[0] = sw.ElapsedMilliseconds;
            sw.Reset();
            service.garmentRepository.CreateBrandIndex();
            sw.Start();
            List<Garment> garments = service.garmentRepository.GetByBrand(brand);
            sw.Stop();
            withIndexes[1] = sw.ElapsedMilliseconds;
            sw.Reset();

            View.OutputHashChart(withoutIndexes, withIndexes);
        }
        public void CreateBtreeChart()
        {
            int garmentCount = service.garmentRepository.GetCount();
            int reviewCount = service.reviewRepository.GetCount();
            if(garmentCount == 0 || reviewCount == 0)
            {
                View.OutputError("There aren`t enough entities to conduct test.");
                return;
            }
            Random ran = new Random();
            int low1 = ran.Next(0, 200);
            int high1 = ran.Next(300, 500);
            double[] withoutIndexes = new double[2];
            Stopwatch sw = new Stopwatch();
            service.garmentRepository.DropBtreeIndex();
            sw.Start();
            List<Garment> garmentsById1 = service.garmentRepository.GetByCostRange(low1, high1);
            sw.Stop();
            withoutIndexes[0] = sw.ElapsedMilliseconds;
            sw.Reset();
            
            int minRating = ran.Next(0, 11);
            DateTime start = new DateTime(2019,1,1);
            DateTime end = new DateTime(2021,1,1);
            TimeSpan range = end-start;
            TimeSpan ts = new TimeSpan((long)(ran.NextDouble() * range.Ticks));
            DateTime minPostedAt = start + ts;
            service.reviewRepository.DropBtreeIndex();
            sw.Start();
            List<Review> reviewsById1 = service.reviewRepository.GetByRatingAndPosted(minRating, minPostedAt);
            sw.Stop();
            withoutIndexes[1] = sw.ElapsedMilliseconds;
            sw.Reset();

            double[] withIndexes = new double[2];
            service.garmentRepository.CreateBtreeIndex();
            sw.Start();
            List<Garment> garments1 = service.garmentRepository.GetByCostRange(low1, high1);
            sw.Stop();
            withIndexes[0] = sw.ElapsedMilliseconds;
            sw.Reset();

            service.reviewRepository.CreateBtreeIndex();
            sw.Start();
            List<Review> reviews2 = service.reviewRepository.GetByRatingAndPosted(minRating, minPostedAt);
            sw.Stop();
            withIndexes[1] = sw.ElapsedMilliseconds;
            sw.Reset();

            View.OutputBtreeChart(withoutIndexes, withIndexes);
        }
        public void GenerateGarmentReport(int garmentId)
        {
            Garment garmentById = service.garmentRepository.GetById(garmentId);
            if(garmentById == null)
            {
                View.OutputError($"Garment with id [{garmentId}] doesn`t exist.");
                return;
            }
            Review highestRatingRev = service.reviewRepository.GetHighestRatingReview(garmentId);
            List<int> ratings = service.reviewRepository.GetGarmentRatings(garmentId);
            if(highestRatingRev == null)
            {
                ReportGenerator.SetValues(garmentById,null,null,null,null,0,ratings);
                ReportGenerator.Run();
                return;
            }
            Review lowestRatingRev = service.reviewRepository.GetLowestRatingReview(garmentId);
            Client highestRatingClient = service.clientRepository.GetById(highestRatingRev.client_id);
            Client lowestRatingClient = service.clientRepository.GetById(lowestRatingRev.client_id);
            double rating = service.reviewRepository.GetGarmentRating(garmentId);

            ReportGenerator.SetValues(garmentById,highestRatingRev,lowestRatingRev,highestRatingClient,lowestRatingClient,rating,ratings);
            ReportGenerator.Run();
        }
        public void CreateIncomeByMonthsChart(int year)
        {
            List<double> incomes = service.orderRepository.GetIncomesByYear(year);
            if(incomes.Count == 0)
            {
                View.OutputError("There are not enough info to do research.");
                return;
            }
            View.OutputYearIncomesChart(incomes, year);
        }
        public void Backup()
        {
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"/home/valeria/Desktop/db_course_work/data/backup/backup.sh {DateTime.Now.ToString().Replace('/','.')}\"",
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                process.Start();
                process.WaitForExit();
                sw.Stop();
                View.OutputBackupInfo($"/home/valeria/Desktop/db_course_work/data/backup/backup.sh {DateTime.Now.ToString().Replace('/','.')}", sw.ElapsedMilliseconds);
            }
            catch(Exception ex)
            {
                View.OutputError(ex.Message);
            }
        }
        public void Restore(string filePath)
        {
            if(!File.Exists(filePath))
            {
                View.OutputError("Entered file path doesn`t exist.");
                return;
            }
            if(!filePath.EndsWith(".sql"))
            {
                View.OutputError("Entered file path is incorrect.");
                return;
            }
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"/home/valeria/Desktop/db_course_work/data/backup/restore.sh {filePath}\"",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    Verb = "sudo"
                }
            };
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                process.Start();
                process.WaitForExit();
                sw.Stop();
                View.OutputRestoreInfo(filePath, sw.ElapsedMilliseconds);
            }
            catch(Exception ex)
            {
                View.OutputError(ex.Message);
            }
            Environment.Exit(0);
        }
    }
}
