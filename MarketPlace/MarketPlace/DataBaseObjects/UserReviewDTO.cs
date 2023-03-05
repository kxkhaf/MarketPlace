namespace MarketPlace;

public class UserReviewDTO
{
    public int Id { get; set; }
    public int ReviewerId { get; set; }
    public int ProductId { get; set; }
    public int Rating { get; set; }
    public string Message { get; set; }
}