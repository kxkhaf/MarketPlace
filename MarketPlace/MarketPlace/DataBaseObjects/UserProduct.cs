namespace MarketPlace;

public class UserProduct
{
    public  int UserId { get; set; }
    public int ProductId { get; set; }
    public  long ProductCount { get; set; }

    public UserProduct(int user_id, int product_id, long product_count)
    {
        UserId = user_id;
        ProductId = product_id;
        ProductCount = product_count;
    }
}
public class UserProductDTO
{
    public  int UserId { get; set; }
    public int ProductId { get; set; }
    public  long ProductCount { get; set; }
}
public class ProductIDCountDTO
{
    public int ProductId { get; set; }
    public  long ProductCount { get; set; }
}

public class ProductIDCountBalanceDTO
{
    public ProductIDCountDTO[] Products { get; set; }
    public long Balance { get; set; }
}