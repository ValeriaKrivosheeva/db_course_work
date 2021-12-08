using System;
using System.Collections.Generic;
namespace Model
{
    public class Client
    {
        public int id { get; set; }
        public string fullname { get; set; }
        public string email { get; set; }
        public DateTime birthday_date { get; set; }
        public List<Review> reviews { get; set; }
        public List<Order> orders { get; set; }
        public override string ToString()
        {
            return string.Format($"[{this.id}]\nFullname: {this.fullname},\nEmail: {this.email}," 
            + $"\nBirth date: {this.birthday_date.ToShortDateString()}.");
        }
    }
}