using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace
{
    public class Review
    {
        public int Id { get; set; }
        public int ReviewerId { get; set; }
        public int ProductId { get; set; }
        public int Rating { get; set; }
        public string Message { get; set; }
        public Review(int id, int reviewer_id, int product_id, int rating, string message)
        {
            Id = id;
            ReviewerId = reviewer_id;
            ProductId = product_id;
            Rating = rating;
            Message = message;
        }
    }
}
