using System;
using System.Collections.Generic;
namespace Model
{
    public class Order
    {
        public int id { get; set; }
        public DateTime created_date { get; set; }
        public string shipping_method { get; set; }
        public int client_id { get; set; }
        public Client client { get; set; }
        public List<Garment> garments { get; set; }
        public List<OrderItem> order_items { get; set; }
        public override string ToString()
        {
            return string.Format($"[{this.id}]\nShipping method: {this.shipping_method},\nCreated date: {this.created_date.ToShortDateString()}," 
            + $"\nClient id: {this.client_id}.");
        }
    }
}