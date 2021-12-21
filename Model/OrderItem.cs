namespace Model
{
    public class OrderItem
    {
        public int id { get; set; }
        public int garment_id { get; set; }
        public int order_id { get; set; }
        public Garment garment { get; set; }
        public Order order { get; set; }
        public override string ToString()
        {
            return string.Format($"Garment id: {this.garment_id},\nOrder id: {this.order_id}.");
        }
    }
}