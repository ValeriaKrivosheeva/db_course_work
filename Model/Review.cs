namespace Model
{
    public class Review
    {
        public int id { get; set; }
        public string opinion { get; set; }
        public int rating { get; set; }
        public int client_id { get; set; }
        public int garment_id { get; set; }
        public Client client { get; set; }
        public Garment garment { get; set; }
        public override string ToString()
        {
            return string.Format($"[{this.id}]\nOpinion: {this.opinion},\nRating: {this.rating}," 
            + $"\nClient id: {this.client_id},\nGarment id: {this.garment_id}.");
        }
    }
}