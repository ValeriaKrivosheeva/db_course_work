using System.Collections.Generic;
namespace Model
{
    public class Garment
    {
        public int id { get; set; }
        public string name { get; set; }
        public string brand { get; set; }
        public int cost { get; set; }
        public string manufacture_country { get; set; }
        public List<Order> orders { get; set; }
        public List<OrderItem> order_items { get; set; }
        public List<Review> reviews { get; set; }
        public override string ToString()
        {
            return string.Format($"[{this.id}]\nName: {this.name},\nBrand: {this.brand}," 
            + $"\nCost: {this.cost},\nManufacture country: {this.manufacture_country}.");
        }
    }
}