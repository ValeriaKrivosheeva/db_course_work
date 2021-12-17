using System;
using Model;
using ViewLib;
using System.Collections.Generic;

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
                View.OutputInsertResult(typeof(Garment).Name, orderId);
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
    }
}
